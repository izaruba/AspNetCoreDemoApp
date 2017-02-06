using System;
using System.Linq;
using System.Security.Claims;
using AspNetCoreDemoApp.Gameplay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemoApp
{
    [Route("game/{id}")]
    [Authorize]
    public class GameRoomController : Controller
    {
        private readonly GameServer gameServer;

        public Player Player => new PlayerIdentity(this.User.Identity as ClaimsIdentity).Player;

        public GameRoomController(GameServer gameServer)
        {
            this.gameServer = gameServer;
        }

        [HttpGet("players")]
        public IActionResult GetPlayers(Guid id)
        {
            if (!this.TryGetRoom(id, out var room, out var error))
            {
                return this.BadRequest(error);
            }

            var lead = room.GameLead;

            return this.Ok(room.Players.Select(player => $"{(player == lead ? "[LEAD] " : "")}{player.Name}"));
        }

        [HttpGet("messages")]
        public IActionResult GetMessages(Guid id)
        {
            if (!this.TryGetRoom(id, out var room, out var error))
            {
                return this.BadRequest(error);
            }

            return this.Ok(room.Messages.Select(m => new { m.PlayerName, m.Text }));
        }

        [HttpPost("say/{text}")]
        public IActionResult Say(Guid id, string text)
        {
            if (!this.TryGetRoom(id, out var room, out var error))
            {
                return this.BadRequest(error);
            }

            try
            {
                room.Say(new Message(this.Player, text));
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }

            return this.Ok();
        }

        private bool TryGetRoom(Guid id, out GameRoom room, out string error)
        {
            try
            {
                room = this.gameServer.GetRoom(id);
            }
            catch (Exception e)
            {
                room = null;
                error = e.Message;
                return false;
            }

            error = null;
            return true;
        }
    }
}