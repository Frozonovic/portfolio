namespace backend.Models
{
    public class GitHubRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Languages { get; set; }
        public string SvnUrl { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}