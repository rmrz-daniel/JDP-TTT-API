using JDP_TTT_API.Models;
using JDP_TTT_API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace JDP_TTT_API.Controllers {

    [Controller]
    [Route("/")]
    public class GamesController : Controller {

        private readonly MongoDBService _mongoDBService;

        public GamesController(MongoDBService mongoDBService) {
            _mongoDBService = mongoDBService;
        }

        [HttpGet("/initgame")]
        public async Task<IActionResult> InitGame() {

            var game = await _mongoDBService.createInitialGameAsync();
            return CreatedAtAction(nameof(InitGame), new { GameID = game.Gameid, Player1_ID = game.player_1, Player2_ID = game.player_2 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> registerMove(string id, [FromBody] moves move) {
            List<moves> moveList = await _mongoDBService.getMoveList(id);
            games gameInfo = await _mongoDBService.getGameInfo(id);

            if (move.player != gameInfo.player_2 && move.player != gameInfo.player_1) {
                return BadRequest("Player id provided does not match up with the registered player id's for the game");
            }

            if (moveList == null) {
                return BadRequest("Move list cannot be null");
            }

            if (moveList.Count > 0 && moveList[moveList.Count - 1].player == move.player) {
                return BadRequest("Player already went");
            }

            if (move.col > 3 || move.col < 1 || move.row > 3 || move.row < 1) {
                return BadRequest("The cordinates are not valid");
            }

            foreach (var m in moveList) {
                if ((m.col, m.row) == (move.col, move.row)) {
                    return BadRequest("This position has already been played");
                };
            }

            await _mongoDBService.createMoveAsync(id, move);
            return CreatedAtAction(nameof(registerMove), "Move registered");
        }


    }
}
