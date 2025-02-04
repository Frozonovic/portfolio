using System.Collections.Generic;

namespace Backend.Models
{
    public class GitHubRepository
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SvnUrl { get; set; } = string.Empty;
        public List<string> Languages { get; set; } = new();
    }
}