using Backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class PollService
    {
        private readonly IMongoCollection<Poll> _pollsCollection;

        public PollService(IOptions<MongoDBSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _pollsCollection = mongoDatabase.GetCollection<Poll>("Polls");
        }

        public async Task<List<Poll>> GetPollsAsync() =>
            await _pollsCollection.Find(_ => true).ToListAsync();

        public async Task CreateOrReplacePollAsync(Poll newPoll)
        {
            // Check for blank or duplicate options
            var optionTexts = new HashSet<string>();
            foreach (var opt in newPoll.Options)
            {
                if (string.IsNullOrWhiteSpace(opt.Text) || !optionTexts.Add(opt.Text.Trim()))
                    throw new ArgumentException("Duplicate or empty options are not allowed.");
            }

            if (string.IsNullOrWhiteSpace(newPoll.Question))
                throw new ArgumentException("Question cannot be empty.");

            // Delete existing polls (assuming single-poll app)
            await _pollsCollection.DeleteManyAsync(_ => true);
            await _pollsCollection.InsertOneAsync(newPoll);
        }

        public async Task<Poll?> UpdateVoteAsync(string selectedOption)
        {
            var poll = await _pollsCollection.Find(_ => true).FirstOrDefaultAsync();
            if (poll == null) return null;

            // Find selected option
            var option = poll.Options.FirstOrDefault(o => o.Text.Equals(selectedOption, StringComparison.OrdinalIgnoreCase));
            if (option == null)
                throw new ArgumentException("Selected option not found.");

            // Increment vote
            option.Votes++;

            // Recalculate percentages
            int totalVotes = poll.Options.Sum(o => o.Votes);
            foreach (var opt in poll.Options)
            {
                opt.Percentage = totalVotes == 0 ? 0 : Math.Round((double)opt.Votes / totalVotes * 100, 2);
            }

            // Update document
            await _pollsCollection.ReplaceOneAsync(x => x.Id == poll.Id, poll);
            return poll;
        }

    }


}
