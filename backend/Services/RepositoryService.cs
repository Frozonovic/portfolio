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
            var token = Environment.GetEnvironmentVariable("TOKEN");
            var username = Environment.GetEnvironmentVariable("USER");

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "backend");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"token {token}");

            var response = await _httpClient.GetAsync($"https://api.github.com/users/{username}/repos");

            if (response.IsSuccessStatusCode)
            {
                var repos = await response.Content.ReadFromJsonAsync<List<Repository>>();
                if (repos != null)
                {
                    _context.Repositories.RemoveRange(_context.Repositories);
                    await _context.Repositories.AddRangeAsync(repos);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}