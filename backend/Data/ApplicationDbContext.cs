using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string DATABASE_URL = Environment.GetEnvironmentVariable("DATABASE_URL");

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(DATABASE_URL))
            {
                optionsBuilder.UseNpgsql(DATABASE_URL);
            }
            else
            {
                throw new InvalidOperationException("Databe connection URL is not set properly.");
            }
        }

        public DbSet<Repository> Repositories { get; set; }
    }
}