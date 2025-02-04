using backend.Data;
using backend.Services;

using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Polly;

// Use internal URLs when available
var DATABASE_PRIVATE_URL = Environment.GetEnvironmentVariable("DATABASE_PRIVATE_URL");
var REDIS_PRIVATE_URL = Environment.GetEnvironmentVariable("REDIS_PRIVATE_URL");

var DATABASE_URL = DATABASE_PRIVATE_URL ?? 
    Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
    "postgresql://postgres:password@localhost:5432/railway";

var REDIS_URL = REDIS_PRIVATE_URL ?? 
    Environment.GetEnvironmentVariable("REDIS_URL") ?? 
    "redis://localhost:6379";

var builder = WebApplication.CreateBuilder(args);

// Add database with retry policy
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(DATABASE_URL, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
    });
});

// Add Redis with configuration
var redisOptions = ConfigurationOptions.Parse(REDIS_URL);
redisOptions.ConnectRetry = 5;
redisOptions.ConnectTimeout = 5000;
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisOptions));

// Add GitHub service
builder.Services.AddHttpClient<IGitHubService, GitHubService>();

// Add background service
builder.Services.AddHostedService<GitHubSyncService>();

// Add controllers
builder.Services.AddControllers();