namespace backend.Services
{
    public interface IRepositoryService
    {
        Task<IEnumerable<Repository>> GetRepositoriesAsync();
        Task SyncRepositoriesAsync();
    }