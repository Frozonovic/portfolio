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

                services.AddSingleton<RedisCacheService>(sp => new RedisCacheService());

                services.addHttpClient();

                services.AddDbContext<ApplicationDbContext>(options => new ApplicationDbContext(options));
            });
    }
}