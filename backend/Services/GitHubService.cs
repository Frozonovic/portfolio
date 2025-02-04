using backend.Models;

namespace backend.Services
{
    public interface IGitHubService
    {
        Task<List<GitHubRepo>> GetUserRepositoriesAsync();
    }

    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;
        private readonly string _username = Environment.GetEnvironmentVariable("USER");
        private readonly string _token = Environment.GetEnvironmentVariable("TOKEN");

        public GitHubService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.UserAgent.Add(new("GithubPortfolio", "1.0"));
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", _token);
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