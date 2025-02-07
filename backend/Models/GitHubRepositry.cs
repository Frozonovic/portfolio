namespace backend.Models
{
    public class GitHubRepository
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Svn_url { get; set; }
        public required List<string> Languages { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Description: {Description ?? "No description available"}, Svn_url: {Svn_url}, Languages: {convertLanguages()}";
        }

        private string convertLanguages()
        {
            var str = "";
            
            foreach (var l in Languages)
            {
                str += l + ", ";
            }

            return str.TrimEnd(',', ' ');
        }
    }
}