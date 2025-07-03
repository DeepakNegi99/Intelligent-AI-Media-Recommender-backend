using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Backend.Models
{
    public class PollOption
    {
        public string Text { get; set; } = string.Empty;
        public int Votes { get; set; } = 0;
        public double Percentage { get; set; } = 0;
    }

    public class Poll
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Question { get; set; } = string.Empty;

        public List<PollOption> Options { get; set; } = new();
    }
}
