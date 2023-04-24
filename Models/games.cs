using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace JDP_TTT_API.Models {
    public class games {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Gameid { get; set; }

        public string player_1 { get; set; } = null!;
        public string player_2 { get; set; } = null!;

        public List<(string, int, int)> moves { get; set; } = null!;





    }
}
