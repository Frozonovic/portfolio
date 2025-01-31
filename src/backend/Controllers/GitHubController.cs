using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public GitHubController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            // Ensure that headers are added only once during initialization
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "backend");
        }

        [HttpGet]
        public async Task<IActionResult> GetGitHubProjects()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://api.github.com/users/Frozonovic/repos");
                if (string.IsNullOrEmpty(response))
                    return NotFound();

                return Ok(response); // Return the GitHub data as a response
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
