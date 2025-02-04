using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend.Services
{
    public class GitHubSyncService : BackgroundService
    {
        private readonly ILogger<GitHubSyncService> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly RedisCacheService _cacheService;
        private const int SYNC_INTERVAL_MINUTES = 60;

        public GitHubSyncService(
            ILogger<GitHubSyncService> logger,
            IRepositoryService repositoryService,
            RedisCacheService cacheService)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _cacheService = cacheService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting GitHub repository sync");
                    await _repositoryService.SyncRepositoriesAsync();
                    await _cacheService.InvalidateRepositoriesCache();
                    _logger.LogInformation("GitHub repository sync completed");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while syncing repositories");
                }

                await Task.Delay(TimeSpan.FromMinutes(SYNC_INTERVAL_MINUTES), stoppingToken);
            }
        }
    }
}