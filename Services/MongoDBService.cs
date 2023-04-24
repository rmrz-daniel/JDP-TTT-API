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
    }
}
