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
            await _mongoDBService.createMoveAsync(id, move);
            

            return NoContent();
        }


    }
}
