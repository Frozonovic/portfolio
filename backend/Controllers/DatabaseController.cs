using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly string _connectionString;

        public DatabaseController()
        {
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? throw new InvalidOperationException("Environment variable 'DATABASE_URL' is not set.");
        }

        [HttpGet]
        public async Task<ActionResult<List<GitHubRepository>>> GetProjectsFromDatabase()
        {
            var projects = new List<GitHubRepository>();

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve projects", error = ex.Message });
            }
        }
    }
}