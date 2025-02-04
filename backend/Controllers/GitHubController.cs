using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly IRepositoryService _repositoryService;

        public GitHubController(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repository>>> GetRepositories()
        {
            var repositories = await _repositoryService.GetRepositoriesAsync();
            return Ok(repositories);
        }
    }
}