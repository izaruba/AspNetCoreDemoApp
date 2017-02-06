using System;
using System.Linq;
using System.Security.Claims;
using AspNetCoreDemoApp.Gameplay;

namespace AspNetCoreDemoApp
{
    public class PlayerIdentity : ClaimsIdentity
    {
        public Player Player
        {
            get
            {
                var id = Guid.TryParse(this.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out var guid) ? guid : Guid.Empty;
                var name = this.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

                return new Player(id, name);
            }
        }

        public PlayerIdentity(ClaimsIdentity identity)
            : base(identity)
        { }
    }
}