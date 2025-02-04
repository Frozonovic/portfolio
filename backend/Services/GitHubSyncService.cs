using backend.Data;
using backend.Services;

using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class GitHubSyncService : BackgroundService
    {
        private readonly IGitHubService _githubService;
        private readonly ICacheService _cacheService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GitHubSyncService> _logger;

        public GitHubSyncService(
            IGitHubService githubService,
            ICacheService cacheService,
            ApplicationDbContext dbContext,
            ILogger<GitHubSyncService> logger)
        {
            _githubService = githubService;
            _cacheService = cacheService;
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var repos = await _githubService.GetUserRepositoriesAsync();

                    // Update cache
                    await _cacheService.SetAsync("github_repos", repos, TimeSpan.FromHours(1));

                    // Update database
                    await _dbContext.GitHubRepos.ExecuteDeleteAsync(stoppingToken);
                    await _dbContext.GitHubRepos.AddRangeAsync(repos, stoppingToken);
                    await _dbContext.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation("GitHub repositories synchronized successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error synchronizing GitHub repositories");
                }

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }
}