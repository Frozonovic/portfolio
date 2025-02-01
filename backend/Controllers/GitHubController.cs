using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class GitHubRepository
{
    public required int id { get; set; }
    public required string name { get; set; }
    public string? description { get; set; }
    public required string svn_url { get; set; }
    public required List<string> languages { get; set; } = new List<string>();
}

[ApiController]
[Route("api/[controller]")]
public class GitHubController : ControllerBase
{
    private readonly HttpClient client;
    private static readonly string token = Enviornment.GetEnvironmentVariable("TOKEN");
    private static readonly string user = Environment.GetEnvironmentVariable("USER");


    public GitHubController(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Add("User-Agent", "backend");
        client.DefaultRequestHeaders.Add("Authorization", $"token {token}");
    }

    [HttpGet]
    public async Task<List<GitHubRepository>> GetGitHubProjects()
    {
        try
        {
            var repoLink = $"https://api.github.com/users/{user}/repos";

            var response = await client.GetStringAsync(repoLink);
            var repos = JsonConvert.DeserializeObject<List<GitHubRepository>>(response);

            foreach (var repo in repos)
            {
                repo.languages = await GetLanguages(repo.name);
            }

            return repos;
        }
        catch (HttpRequestException e)
        {
            return StatisCode(500, new { message = "Failed to fetch repositories", error = e.Message });
        }


    }

    private async Task<List<string>> GetLanguages(string repoName)
    {
        try
        {
            var langLink = $"https://api.github.com/repos/{user}/{repoName}/languages";

            var response = await client.GetStringAsync(langLink);
            var langs = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            return new List<string>(langs.Keys);
        }
        catch
        {
            return new List<string>();
        }
    }
}