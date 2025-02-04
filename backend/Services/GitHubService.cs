using backend.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backend.Services
{
    public interface IGitHubService
    {
        Task<List<GitHubRepo>> GetUserRepositoriesAsync();
    }

    public class GitHubSettings
    {
        public string Username { get; set; } = null!;
        public string Token { get; set; } = null!;
    }

    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly GitHubSettings _settings;
        private readonly ILogger<GitHubService> _logger;

        public GitHubService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GitHubService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Bind configuration section to strongly-typed settings
            _settings = configuration.GetSection("GitHub").Get<GitHubSettings>() ??
                throw new InvalidOperationException("GitHub settings not configured");

            _httpClient.DefaultRequestHeaders.UserAgent.Add(new("GithubPortfolio", "1.0"));
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", _settings.Token);
        }

        public async Task<List<GitHubRepo>> GetUserRepositoriesAsync()
        {
            var response = await _httpClient.GetAsync($"https://api.github.com/users/{_username}/repos");
            response.EnsureSuccessStatusCode();
            var repos = await response.Content.ReadFromJsonAsync<List<GitHubRepo>>();
            return repos ?? new List<GitHubRepo>();
        }
    }
}