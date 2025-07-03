using Backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class UserPreferenceService
    {
        private readonly IMongoCollection<UserPreferences> _preferencesCollection;

        public UserPreferenceService(IOptions<MongoDBSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _preferencesCollection = database.GetCollection<UserPreferences>("UserPreferences");
        }

        public async Task CreateAsync(UserPreferences preferences)
        {
            await _preferencesCollection.InsertOneAsync(preferences);
        }

        public async Task<List<UserPreferences>> GetAllAsync() =>
            await _preferencesCollection.Find(_ => true).ToListAsync();
    }
}
