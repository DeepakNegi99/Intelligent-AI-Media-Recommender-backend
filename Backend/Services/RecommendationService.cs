using Backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Backend.Services
{
    public class RecommendationService
    {
        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMongoCollection<Movie> _movieCollection;
        private readonly IMongoCollection<TVSeries> _tvSeriesCollection;

        public RecommendationService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);

            _bookCollection = db.GetCollection<Book>("Books");
            _movieCollection = db.GetCollection<Movie>("Movies");
            _tvSeriesCollection = db.GetCollection<TVSeries>("TVSeries");
        }

        public async Task<List<RecommendationItem>> GetRecommendationsAsync(UserPreferences preferences)
        {
            var books = await _bookCollection.Find(_ => true).ToListAsync();
            var movies = await _movieCollection.Find(_ => true).ToListAsync();
            var tvSeries = await _tvSeriesCollection.Find(_ => true).ToListAsync();

            var allItems = new List<RecommendationItem>();

            // Convert and score each type
            allItems.AddRange(books.Select(b => ScoreMatch(b, preferences)));
            allItems.AddRange(movies.Select(m => ScoreMatch(m, preferences)));
            allItems.AddRange(tvSeries.Select(t => ScoreMatch(t, preferences)));

            return allItems
                .Where(i => i.Score > 0)
                .OrderByDescending(i => i.Score)
                .ToList();
        }

        private RecommendationItem ScoreMatch(dynamic item, UserPreferences preferences)
        {
            int score = 0;

            // Cast item.Genres to IEnumerable<string> to resolve CS1977
            if (item.Genres is IEnumerable<string> genres && genres.Any(g => preferences.FavoriteGenres.Contains(g))) score++;

            if (!string.IsNullOrWhiteSpace(item.Mood) && item.Mood == preferences.Mood) score++;

            // Cast item.Platforms to IEnumerable<string> to resolve CS1977
            if (item.Platforms is IEnumerable<string> platforms && platforms.Any(p => preferences.Platforms.Contains(p))) score++;

            return new RecommendationItem
            {
                Title = item.Title,
                Type = item.GetType().Name,
                Score = score
            };
        }
    }

    public class RecommendationItem
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Book / Movie / TVSeries
        public int Score { get; set; }
    }
}
