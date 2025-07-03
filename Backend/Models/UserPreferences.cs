using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Backend.Models
{
    public class UserPreferences
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string UserId { get; set; } = string.Empty;  // for future personalization

        public List<string> FavoriteGenres { get; set; } = new();
        public string Mood { get; set; } = string.Empty;
        public List<string> FavoriteAuthors { get; set; } = new();
        public List<string> PreferredLanguages { get; set; } = new();
        public List<string> Platforms { get; set; } = new();
    }
}
