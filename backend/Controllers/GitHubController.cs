using backend.Data;
using backend.Models;
using backend.Services;

using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly ApplicationDbContext _dbContext;

        public GitHubController(ICacheService cacheService, ApplicationDbContext dbContext)
        {
            _cacheService = cacheService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<GitHubRepo>>> GetRepositories()
        {
            // Try cache first
            var cachedRepos = await _cacheService.GetAsync<List<GitHubRepo>>("github_repos");
            if (cachedRepos != null)
                return cachedRepos;

            // Fallback to database
            var repos = await _dbContext.GitHubRepos.ToListAsync();
            return repos;
        }
    }
}