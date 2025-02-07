using backend.Models;
using Microsoft.Extensions.Hosting;
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
        private readonly string TOKEN;
        private readonly string USER;
        private readonly string PGHOST;
        private readonly string PGPORT;
        private readonly string PGUSER;
        private readonly string PGPASSWORD;
        private readonly string PGDATABASE;

        private readonly HttpClient _httpClient;

        public GitHubUpdateService()
        {
            Console.WriteLine("Setting up GitHubUpdateService...");

            TOKEN = Environment.GetEnvironmentVariable("TOKEN") ?? throw new InvalidOperationException("Environment variable TOKEN is not set.");
            USER = Environment.GetEnvironmentVariable("USER") ?? throw new InvalidOperationException("Environment variable USER is not set.");
            PGHOST = Environment.GetEnvironmentVariable("PGHOST") ?? throw new InvalidOperationException("Environment variable PGHOST is not set.");
            PGPORT = Environment.GetEnvironmentVariable("PGPORT") ?? throw new InvalidOperationException("Environment variable PGPORT is not set.");
            PGUSER = Environment.GetEnvironmentVariable("PGUSER") ?? throw new InvalidOperationException("Environment variable  PGUSER is not set.");
            PGPASSWORD = Environment.GetEnvironmentVariable("PGPASSWORD") ?? throw new InvalidOperationException("Environment variable PGPASSWORD is not set.");
            PGDATABASE = Environment.GetEnvironmentVariable("PGDATABASE") ?? throw new InvalidOperationException("Environment variable PGDATABASE is not set.");

            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "backend");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {TOKEN}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Running ExecuteAsync()...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateDatabase();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task UpdateDatabase()
        {
            Console.WriteLine("Running UpdateDatabase()...");

            try
            {
                var repoLink = $"https://api.github.com/users/{USER}/repos";
                var response = await _httpClient.GetStringAsync(repoLink);
                var repos = JsonConvert.DeserializeObject<List<GitHubRepository>>(response);

                if (repos == null) { return; }

                using var conn = new NpgsqlConnection($"Host={PGHOST};Port={PGPORT};Username={PGUSER};Password={PGPASSWORD};Database={PGDATABASE}");
                await conn.OpenAsync();

                foreach (var repo in repos)
                {
                    repo.Languages = await GetLanguages(repo.Name);

                    Console.WriteLine(repo);

                    var query = @"
                        INSERT INTO repositories (id, name, description, svn_url, languages)
                        VALUES (@id, @name, @desc, @svn, @langs)
                        ON CONFLICT (id)
                        DO UPDATE SET 
                            name = EXCLUDED.name,
                            description = EXCLUDED.description,
                            svn_url = EXCLUDED.svn_url,
                            languages = EXCLUDED.languages;";

                    await using var cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("id", repo.Id);
                    cmd.Parameters.AddWithValue("name", repo.Name);
                    cmd.Parameters.AddWithValue("desc", (object?)repo.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("svn", repo.Svn_url);
                    cmd.Parameters.AddWithValue("langs", repo.Languages.ToArray());

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("🚨 An exception occurred in GitHubUpdateService.cs! 🚨");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                throw;
            }
        }

        private async Task<List<string>> GetLanguages(string repoName)
        {
            Console.WriteLine("Running GetLanguages()...");

            try
            {
                var langLink = $"https://api.github.com/repos/{USER}/{repoName}/languages";
                var response = await _httpClient.GetStringAsync(langLink);
                var langs = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);

                Console.WriteLine("Returning languages...");

                return langs?.Keys.ToList() ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("🚨 A different exception occurred in GitHubUpdateService.cs! 🚨");

                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                throw;
            }
        }
    }
}