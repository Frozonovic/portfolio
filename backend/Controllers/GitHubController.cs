using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class GitHubRepository
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SvnUrl { get; set; } = string.Empty;
    public List<string> Languages { get; set; } = new();
}

/// <summary>
/// Controller for handling GitHub API requests
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GitHubController : ControllerBase
{
    private readonly HttpClient _client;
    private static readonly string _token = Environment.GetEnvironmentVariable("TOKEN") ??
        throw new InvalidOperationException("GitHub token not found in environment variables");
    private static readonly string _user = Environment.GetEnvironmentVariable("USER") ??
        throw new InvalidOperationException("GitHub user not found in environment variables");

    public GitHubController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "backend");
        _client.DefaultRequestHeaders.Add("Authorization", $"token {_token}");
    }

    /// <summary>
    /// Retrieves GitHub projects for the configured user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<GitHubRepository>>> GetGitHubProjects()
    {
        try
        {
            var repoLink = $"https://api.github.com/users/{_user}/repos";
            var response = await _client.GetStringAsync(repoLink);
            var repos = JsonConvert.DeserializeObject<List<GitHubRepository>>(response);

            if (repos == null)
                return NotFound("No repositories found");

            foreach (var repo in repos)
            {
                repo.Languages = await GetLanguages(repo.Name);
            }

            return Ok(repos);
        }
        catch (HttpRequestException e)
        {
            return StatusCode(500, new { message = "Failed to fetch repositories", error = e.Message });
        }
    }

    private async Task<List<string>> GetLanguages(string repoName)
    {
        try
        {
            var langLink = $"https://api.github.com/repos/{_user}/{repoName}/languages";
            var response = await _client.GetStringAsync(langLink);
            var langs = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            return langs?.Keys.ToList() ?? new List<string>();
        }
        catch (Exception)
        {
            return new List<string>();
        }
    }
}