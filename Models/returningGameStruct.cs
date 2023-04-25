
namespace JDP_TTT_API.Models {
    public class returningGameStruct {
        public string? Gameid { get; set; }
        public string player_1 { get; set; } = null!;
        public string player_2 { get; set; } = null!;
        public int moves { get; set; } = 0;
    }
}
