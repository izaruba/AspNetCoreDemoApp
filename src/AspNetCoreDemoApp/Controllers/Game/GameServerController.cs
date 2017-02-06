using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreDemoApp.Gameplay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemoApp
{
    [Route("game")]
    public class GameServerController : Controller
    {
        private readonly AuthenticationService authenticationService;
        private readonly GameServer gameServer;

        public Player Player => new PlayerIdentity(this.User.Identity as ClaimsIdentity).Player;

        public GameServerController(AuthenticationService authenticationService, GameServer gameServer)
        {
            this.authenticationService = authenticationService;
            this.gameServer = gameServer;
        }

        [HttpPost("join"), ValidateModelState]
        public async Task<IActionResult> JoinAsync([FromBody] ConnectPlayerRequest request)
        {
            var player = new Player(Guid.NewGuid(), request.PlayerName);
            var roomId = this.gameServer.Connect(player);

            await this.authenticationService.LogInAsync(player);

            return this.Ok(roomId);
        }

        [HttpPost("leave"), ValidateModelState, Authorize]
        public async Task<IActionResult> LeaveAsync()
        {
            this.gameServer.Disconnect(this.Player);
            await this.authenticationService.LogOutAsync();

            return this.Ok();
        }
    }
}