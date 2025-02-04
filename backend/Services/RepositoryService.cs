using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RepositoryService> _logger;

        public RepositoryService(
            ApplicationDbContext context,
            HttpClient httpClient,
            ILogger<RepositoryService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesAsync()
        {
            try
            {
                var repositories = await _context.Repositories.ToListAsync();
                if (!repositories.Any())
                {
                    await SyncRepositoriesAsync();
                    repositories = await _context.Repositories.ToListAsync();
                }
                return repositories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving repositories");
                throw;
            }
        }

        public async Task SyncRepositoriesAsync()
        {
            try
            {
                var token = Environment.GetEnvironmentVariable("TOKEN");
                var username = Environment.GetEnvironmentVariable("USER");

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(username))
                {
                    throw new InvalidOperationException("GitHub credentials not configured");
                }

                // Clear existing headers to prevent duplicates
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "backend");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");

                var maxRetries = 3;
                var delay = TimeSpan.FromSeconds(2);

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        var response = await _httpClient.GetAsync($"https://api.github.com/users/{username}/repos");

                        _logger.LogInformation($"GitHub API Response Status: {response.StatusCode}");

                        if (!response.IsSuccessStatusCode)
                        {
                            var error = await response.Content.ReadAsStringAsync();
                            _logger.LogError($"GitHub API error: {error}");
                            throw new HttpRequestException($"GitHub API returned {response.StatusCode}: {error}");
                        }

                        var repos = await response.Content.ReadFromJsonAsync<List<Repository>>();

                        if (repos == null || !repos.Any())
                        {
                            _logger.LogWarning("No repositories returned from GitHub API");
                            return;
                        }

                        await using var transaction = await _context.Database.BeginTransactionAsync();
                        try
                        {
                            _context.Repositories.RemoveRange(_context.Repositories);
                            await _context.SaveChangesAsync();

                            await _context.Repositories.AddRangeAsync(repos);
                            await _context.SaveChangesAsync();

                            await transaction.CommitAsync();
                            _logger.LogInformation($"Successfully synced {repos.Count} repositories");
                            return;
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw new Exception("Failed to update database", ex);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        if (i == maxRetries - 1) throw;
                        _logger.LogWarning($"Retry {i + 1}/{maxRetries} after error: {ex.Message}");
                        await Task.Delay(delay);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync repositories");
                throw;
            }
        }
    }
}