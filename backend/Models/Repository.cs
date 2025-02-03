namespace backend.Models
{
    public class Repository
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string svn_url { get; set; }
        public List<string> languages { get; set; } = new List<string>();
    }
}