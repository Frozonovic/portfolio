using backend.Data;
using backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<GitHubSyncService>();

                services.AddSingleton<RedisCacheService>(sp => new RedisCacheService(hostContext.Configuration["REDIS_CONNECTION_STRING"]));

                services.addHttpClient();

                services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(hostContext.Configureation.GetConnectionString("PostgresConnectionString")));
            });
    }
}