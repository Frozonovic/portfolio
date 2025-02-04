using backend.Models;

using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<GitHubRepo> GitHubRepos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Railway provides DATABASE_URL environment variable
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                optionsBuilder.UseNpgsql(databaseUrl);
            }
        }
    }
}