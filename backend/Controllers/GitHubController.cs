using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly HttpClient _client;
        private const string GithubApiBaseUrl = "https://api.github.com";
        private static readonly string _token = Environment.GetEnvironmentVariable("TOKEN") ??
            throw new InvalidOperationException("GitHub token not found");
        private static readonly string _user = Environment.GetEnvironmentVariable("USER") ??
            throw new InvalidOperationException("GitHub user not found");

        public GitHubController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "backend");
            _client.DefaultRequestHeaders.Add("Authorization", $"token {_token}");
        }

        /// <summary>
        /// Retrieves GitHub repositories for the configured user
        /// </summary>
        /// <returns>List of GitHub repositories with their languages</returns>
        /// <response code="200">Returns the list of repositories</response>
        /// <response code="404">If no repositories are found</response>
        /// <response code="500">If there was an internal error</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<GitHubRepository>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<GitHubRepository>>> GetGitHubProjects()
        {
            try
            {
                var repoLink = $"{GithubApiBaseUrl}/users/{_user}/repos";
                var response = await _client.GetStringAsync(repoLink);
                var repos = JsonConvert.DeserializeObject<List<GitHubRepository>>(response);

                if (repos?.Count == 0)
                    return NotFound("No repositories found");

                foreach (var repo in repos!)
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
                var langLink = $"{GithubApiBaseUrl}/repos/{_user}/{repoName}/languages";
                var response = await _client.GetStringAsync(langLink);
                var langs = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);
                return langs?.Keys.ToList() ?? new List<string>();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}