namespace backend.Models
{
    public class GitHubRepo
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required List<string>? Languages { get; set; }
        public required string SvnUrl { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}