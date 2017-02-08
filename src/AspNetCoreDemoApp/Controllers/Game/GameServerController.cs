using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreDemoApp.Gameplay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemoApp
{
    [Route("game"), Authorize]
    public class GameServerController : Controller
    {
        //private readonly AuthenticationService authenticationService;
        private readonly GameServer gameServer;

        public Player Player => new PlayerIdentity(this.User.Identity as ClaimsIdentity).Player;

        public GameServerController(GameServer gameServer)
        {
            this.gameServer = gameServer;
        }

        [HttpPost("join"), ValidateModel]
        public IActionResult Join([FromBody] ConnectPlayerRequest request)
        {
            var player = new Player(Guid.NewGuid(), request.PlayerName);
            var roomId = this.gameServer.Connect(player);

            return this.Ok(roomId);
        }

        [HttpPost("leave"), ValidateModel, Authorize]
        public IActionResult Leave()
        {
            this.gameServer.Disconnect(this.Player);

            return this.Ok();
        }
    }
}