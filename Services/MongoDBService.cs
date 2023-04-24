using JDP_TTT_API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace JDP_TTT_API.Services {
    public class MongoDBService {

        private readonly IMongoCollection<games> _gamesCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _gamesCollection = database.GetCollection<games>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<games> createInitialGameAsync() {

            games initGame = new games();
            initGame.player_1 = Guid.NewGuid().ToString();
            initGame.player_2 = Guid.NewGuid().ToString();

            await _gamesCollection.InsertOneAsync(initGame);
            return initGame;
        }

        public async Task createMoveAsync(string id, moves move) {
            FilterDefinition<games> filter = Builders<games>.Filter.Eq("Gameid", id);
            UpdateDefinition<games> update = Builders<games>.Update.AddToSet("moves", move );
            await _gamesCollection.UpdateOneAsync(filter, update);
            return;
        }
    }
}
