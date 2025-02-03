using StackExchange.Redis;
using System.Threading.Tasks;

namespace backend.Services
{
    public class RedisCacheService
    {
        private readonly string REDIS_URL = Environment.GetEnvironmentVariable("REDIS_URL");

        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;

        public RedisCacheService()
        {
            _redisConnection = ConnectionMultiplexer.Connect(REDIS_URL);
            _database = _redisConnection.GetDatabase();
        }

        public async Task<string> GetCacheAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task SetCacheAsync(string key, string value)
        {
            await _database.StringSetAsync(key, value, TimeSpan.FromHours(1));
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
