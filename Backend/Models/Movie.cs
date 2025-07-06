namespace Backend.Models
{
    public class Movie
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Genres { get; set; } = new();
        public string Mood { get; set; } = string.Empty;
        public List<string> Platforms { get; set; } = new();
    }
}
