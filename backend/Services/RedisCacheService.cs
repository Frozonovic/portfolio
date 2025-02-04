using backend.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace backend.Services
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisCacheService> _logger;
        private const string REPO_CACHE_KEY = "github:repositories";
        private const int CACHE_EXPIRY_HOURS = 24;

        public RedisCacheService(ILogger<RedisCacheService> logger)
        {
            _redis = ConnectionMultiplexer.Connect(Enviornment.GetEnvironmentVariable("REDIS_URL"));
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (value.IsNull)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var db = _redis.GetDatabase();
            var serializedValue = JsonSerializer.Serialize(value);
            await db.StringSetAsync(
                key,
                serializedValue,
                expiry ?? TimeSpan.FromHours(CACHE_EXPIRY_HOURS)
            );
        }

        public async Task<IEnumerable<Repository>> GetRepositoriesAsync()
        {
            return await GetAsync<IEnumerable<Repository>>(REPO_CACHE_KEY);
        }

        public async Task SetRepositoriesAsync(IEnumerable<Repository> repositories)
        {
            await SetAsync(REPO_CACHE_KEY, repositories);
        }

        public async Task InvalidateRepositoriesCache()
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(REPO_CACHE_KEY);
        }
    }
}