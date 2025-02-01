using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class GitHubRepository
{
    public required string id { get; set; }
    public required string name { get; set; }
    public required string description { get; set; }
    public required string svn_url { get; set; }
    public required List<string> languages { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class GitHubController : ControllerBase
{
    private readonly string token = Environment.GetEnvironmentVariable("TOKEN");
    private readonly string user = Environment.GetEnvironmentVariable("USER");
    private readonly HttpClient client;

    public GitHubController(IHttpClientFactory httpClientFactory)
    {
        client = httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Add("User-Agent", "backend");
        client.DefaultRequestHeaders.Add("Authorization", $"token {token}");
    }

    [HttpGet]
    public async Task<List<GitHubRepository>> GetRepositories()
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

    private async Task<List<string>> GetLanguages(string repoName)
    {
        var langLink = $"https://api.github.com/repos/{user}/{repoName}/languages";

        var response = await client.GetStringAsync(langLink);
        var langs = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

        return new List<string>(langs.Keys);
    }
}