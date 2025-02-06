using backend.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace backend.Services
{
    public class GitHubUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly string _user;
        private readonly string _connectionString;

        public GitHubUpdateService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _httpClient = new HttpClient();

            _token = Environment.GetEnvironmentVariable("TOKEN") ?? throw new InvalidOperationException("Environment variable TOKEN is not set.");
            _user = Environment.GetEnvironmentVariable("USER") ?? throw new InvalidOperationException("Environment variable USER is not set.");
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? throw new InvalidOperationException("Environment variable DATABASE_URL is not set.");

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "backend");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {_token}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateDatabase();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task UpdateDatabase()
        {
            try
            {
                var repoLink = $"https://api.github.com/users/{_user}/repos";
                var response = await _httpClient.GetStringAsync(repoLink);
                var repos = JsonConvert.DeserializeObject<List<GitHubRepository>>(response);

                if (response == null) { return; }

                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                foreach (var repo in repos)
                {
                    repo.Languages = await GetLanguages(repo.Name);

                    var query = @"
                        INSERT INTO repositories (id, name, description, svn_url, languages)
                        VALUES (@id, @name, @desc, @svn, @langs)
                        ON CONFLICT (id) DO UPDATE 
                        SET name = EXCLUDED.name, description = EXCLUDED.description, svn_url = EXCLUDED.svn_url, languages = EXCLUDED.languages;";

                    await using var cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", repo.Id);
                    cmd.Parameters.AddWithValue("name", repo.Name);
                    cmd.Parameters.AddWithValue("desc", (object?)repo.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("svn", repo.Svn_url);
                    cmd.Parameters.AddWithValue("langs", repo.Languages.ToArray());

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error updating database: {e.Message}");
            }
        }

        private async Task<List<string>> GetLanguages(string repoName)
        {
            try
            {
                var langLink = $"https://api.github.com/repos/{_user}/{repoName}/languages";
                var response = await _httpClient.GetStringAsync(langLink);
                var langs = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);

                return langs?.Keys.ToList() ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}