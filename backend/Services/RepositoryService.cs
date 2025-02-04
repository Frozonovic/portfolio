using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RepositoryService> _logger;

        public RepositoryService(
            ApplicationDbContext context,
            IConfiguration configuration,
            HttpClient httpClient,
            ILogger<RepositoryService> logger)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesAsync()
        {
            return await _context.Repositories.ToListAsync();
        }

        public async Task SyncRepositoriesAsync()
        {
            var username = Enviornment.GetEnvironmentVariable("USER");
            var token = Environment.GetEnvironmentVariable("TOKEN");

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