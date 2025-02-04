namespace backend.Models
{
    public class GitHubRepository
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Svn_Url { get; set; }
        public required List<string> Languages { get; set; } = [];
    }
}