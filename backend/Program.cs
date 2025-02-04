using backend.Data;
using backend.Services;

using StackExchance.Redis;

var DATABASE_URL = Environment.GetEnvironmentVariable("DATABASE_URL");
var REDIS_URL = Environment.GetEnvironmentVariable("REDIS_URL");

var builder = WebApplication.CreateBuilder(args);

// Add database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(DATABASE_URL));

// Add Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(REDIS_URL));
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// Add GitHub service
builder.Services.AddHttpClient<IGitHubService, GitHubService>();

// Add background service
builder.Services.AddHostedService<GitHubSyncService>();

// Add controllers
builder.Services.AddControllers();