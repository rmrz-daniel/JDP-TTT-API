using JDP_TTT_API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<UpdateResult> createMoveAsync(string gameId, moves move) {
            FilterDefinition<games> filter = Builders<games>.Filter.Eq("Gameid", gameId);
            UpdateDefinition<games> update = Builders<games>.Update.AddToSet("moves", move);

            return await _gamesCollection.UpdateOneAsync(filter, update);
        }

        public async Task<List<moves>> getMoveList(string gameId) {
            FilterDefinition<games> filter = Builders<games>.Filter.Eq("Gameid", gameId);
            var game = await _gamesCollection.Find(filter).FirstAsync();

            return game.moves;
        }

        public async Task<games> getGameInfo(string gameId) {
            FilterDefinition<games> filter = Builders<games>.Filter.Eq("Gameid", gameId);
            var game = await _gamesCollection.Find(filter).FirstAsync();

            return game;
        }
    }
}
