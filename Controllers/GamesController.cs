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

        [HttpPut("/registermove/{id}")]
        public async Task<IActionResult> registerMove(string id, [FromBody] moves move) {
            List<moves> moveList = await _mongoDBService.getMoveList(id);
            games gameInfo = await _mongoDBService.getGameInfo(id);
            string[,] board = new string[3, 3]{
                { "-", "-", "-" },
                { "-", "-", "-" },
                { "-", "-", "-" }
            };

            if (move.player != gameInfo.player_2 && move.player != gameInfo.player_1) {
                return BadRequest( new { Error = "Player id provided does not match up with the registered player id's for the game" });
            }

            if (moveList == null) {
                return BadRequest( new { Error = "Move list cannot be null" });
            }

            if (moveList.Count > 0 && moveList[moveList.Count - 1].player == move.player) {
                return BadRequest( new { Error = "Player already went" });
            }

            if (move.col > 3 || move.col < 1 || move.row > 3 || move.row < 1) {
                return BadRequest( new { Error = "The cordinates are not valid" });
            }

            if (moveList.Count == 9) {
                return BadRequest( new { Error = "This game has already finished" } );
            }

            foreach (var m in moveList) {
                if ((m.col, m.row) == (move.col, move.row)) {
                    return BadRequest( new { Error = "This position has already been played" });
                };
            }

            board[move.row - 1, move.col - 1] = move.player;

            foreach (var m in moveList) {
                board[m.row - 1, m.col - 1] = m.player;
            }

            await _mongoDBService.createMoveAsync(id, move);

            if(winCheck(move.player,board) == false && moveList.Count == 8) {
                await _mongoDBService.updateRunningStatus(id, false);
                return CreatedAtAction(nameof(registerMove), new { success = true, status = "tie"});
            }

            if (winCheck(move.player, board)) {
                await _mongoDBService.updateRunningStatus(id, false);
                return CreatedAtAction(nameof(registerMove), new { success = true, status = "win", winner = move.player });
            }

            return CreatedAtAction(nameof(registerMove), new { success = true });
        }

        private bool winCheck(string playerId, string[,] board) {
            // check row
            for(int row = 0; row < 3; row++) {
                if (board[row, 0] == playerId && board[row, 1] == playerId && board[row, 2] == playerId) {
                    return true;
                }
            }

            // check cols
            for (int col = 0; col < 3; col++) {
                if (board[0, col] == playerId && board[1, col] == playerId && board[2, col] == playerId) {
                    return true;
                }
            }

            // Check diagonal
            if (board[0, 0] == playerId && board[1, 1] == playerId && board[2, 2] == playerId) {
                return true;
            }

            // Check anti-diagonal
            if (board[0, 2] == playerId && board[1, 1] == playerId && board[2, 0] == playerId) {
                return true;
            }

            return false;
        }


    }
}
