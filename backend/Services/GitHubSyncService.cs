using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend.Services
{
    public class GitHubSyncService : BackgroundService
    {
        private readonly string USER = Environment.GetEnvironmentVariable("USER");
        private readonly string TOKEN = Environment.GetEnvironmentVariable("TOKEN");

        private readonly ILogger<GitHubSyncService> _logger;
        private readonly RedisCacheService _redisCacheService;
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public GitHubSyncService(ILogger<GitHubSyncService> logger, RedisCacheService redisCacheService, ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _redisCacheService = redisCacheService;
            _dbContext = dbContext;
            _httpClient = httpClientFactory.CreateClient();

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "backend");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {TOKEN}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var interval = TimeSpan.FromHours(1);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"GitHub sync service started at: {DateTimeOffset.Now}");

                try
                {
                    var response = await _httpClient.GetAsync($"https://api.github.com/users/{USER}/repos");
                    var repos = JsonConvert.DeserializeObject<List<Repository>>(response);

                    if (response != null && response.Any())
                    {
                        foreach (var repo in repos)
                        {
                            var existingRepo = _dbContext.Repositories.FirstOrDefault(r => r.id == repo.id);
                            if (existingRepo == null)
                            {
                                _dbContext.Repositories.Add(repo);
                            }
                            else
                            {
                                existingRepo.name = repo.name;
                                existingRepo.description = repo.description;
                                existingRepo.svn_url = repo.svn_url;
                                existingRepo.languages = repo.languages;
                            }
                        }

                        await _dbContext.SaveChangesAsync();

                        var reposJson = JsonConvert.SerializeObject(repos);
                        await _redisCacheService.SetCacheAsync("github_projects_cache_key", reposJson);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during GitHub sync: {ex.Message}");
                }
            }

            await Task.Delay(interval, stoppingToken);
        }
    }
}