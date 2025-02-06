using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly string PGHOST;
        private readonly string PGPORT;
        private readonly string PGUSER;
        private readonly string PGPASSWORD;
        private readonly string PGDATABASE;

        public DatabaseController()
        {
            PGHOST = Environment.GetEnvironmentVariable("PGHOST") ?? throw new InvalidOperationException("Environment variable PGHOST is not set.");
            PGPORT = Environment.GetEnvironmentVariable("PGPORT") ?? throw new InvalidOperationException("Environment variable PGPORT is not set.");
            PGUSER = Environment.GetEnvironmentVariable("PGUSER") ?? throw new InvalidOperationException("Environment variable  PGUSER is not set.");
            PGPASSWORD = Environment.GetEnvironmentVariable("PGPASSWORD") ?? throw new InvalidOperationException("Environment variable PGPASSWORD is not set.");
            PGDATABASE = Environment.GetEnvironmentVariable("PGDATABASE") ?? throw new InvalidOperationException("Environment variable PGDATABASE is not set.");
        }

        [HttpGet]
        public async Task<ActionResult<List<GitHubRepository>>> GetProjectsFromDatabase()
        {
            var projects = new List<GitHubRepository>();

            try
            {
                await using var conn = new NpgsqlConnection($"Host={PGHOST};Port={PGPORT};Username={PGUSER};Password={PGPASSWORD};Database={PGDATABASE}");
                await conn.OpenAsync();

                var query = "SELECT * FROM repositories";

                await using var cmd = new NpgsqlCommand(query, conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    projects.Add(new GitHubRepository
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "No description available" : reader.GetString(2),
                        Svn_url = reader.GetString(3),
                        Languages = reader.IsDBNull(4) ? new List<string>() : reader.GetFieldValue<string[]>(4).ToList()
                    });
                }

                return Ok(projects);
            }
            catch
            {
                Console.WriteLine("An exception occurred in DatabaseController.cs!");
                return StatusCode(500);
            }
        }
    }
}