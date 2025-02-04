using backend.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly HttpClient client;
        private static readonly string token = Environment.GetEnvironmentVariable("TOKEN") ?? throw new InvalidOperationException("Environment variable TOKEN does not exist.");
        private static readonly string user = Environment.GetEnvironmentVariable("USER") ?? throw new InvalidOperationException("Environment variable USER does not exist.");


        public GitHubController(IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("User-Agent", "backend");
            client.DefaultRequestHeaders.Add("Authorization", $"token {token}");
        }

        [HttpGet]
        public async Task<ActionResult<List<GitHubRepository>>> GetGitHubProjects()
        {
            try
            {
                var repoLink = $"https://api.github.com/users/{user}/repos";

                var response = await client.GetStringAsync(repoLink);
                var repos = JsonConvert.DeserializeObject<List<GitHubRepository>>(response);

                if (repos == null)
                {
                    return NotFound();
                }

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
                var langLink = $"https://api.github.com/repos/{user}/{repoName}/languages";

                var response = await client.GetStringAsync(langLink);
                var langs = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                if (langs == null)
                {
                    return [];
                }

                return [.. langs.Keys];
            }
            catch
            {
                return [];
            }
        }
    }
}