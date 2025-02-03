namespace backend.Models
{
    public class Repository
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string svn_url { get; set; }
        public List<string> languages { get; set; } = new List<string>();

        public Repository(int id, string name, string? description, string svn_url)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.svn_url = svn_url;
        }
    }
}