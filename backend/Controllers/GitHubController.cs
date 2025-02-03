using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly string USER = Environment.GetEnvironmentVariable("USER");
        private readonly string TOKEN = Environment.GetEnvironmentVariable("TOKEN");

        private readonly RedisCacheService _redisCacheService;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public GitHubController(RedisCacheService redisCacheService, ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _redisCacheService = redisCacheService;
            _dbContext = dbContext;
            _httpClient = httpClientFactory.CreateClient();

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "backend");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {TOKEN}");
        }

        [HttpGet]
        public async Task<IActionResult> GetGetHubProjects()
        {
            var cacheKey = "github_projects_cache_key";

            var cachedData = await _redisCacheService.GetCacheAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Ok(JsonConvert.DeserializeObject<List<Repository>>(cachedData));
            }

            var reposFromDb = await _dbContext.Repositories.ToListAsync();
            if (reposFromDb.Any())
            {
                var reposJson = JsonConvert.SerializeObject(reposFromDb);
                await _redisCacheService.SetCacheAsync(cacheKey, reposJson);
                return Ok(reposFromDb);
            }

            var response = await _httpClient.GetAsync($"https://api.github.com/users/{USER}/repos");
            var repos = JsonConvert.DeserializeObject<List<Repository>>(response);

            if (response != null)
            {
                _dbContext.Repositories.AddRange(repos);
                await _dbContext.SaveChangesAsync();

                var reposJson = JsonConvert.SerializeObject(repos);
                await _redisCacheService.SetCacheAsync(cacheKey, reposJson);
            }

            return Ok(repos);
        }
    }
}