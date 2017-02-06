using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AspNetCoreDemoApp.Gameplay;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;

namespace AspNetCoreDemoApp
{
    public class AuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task LogInAsync(Player player)
        {
            var claims = new[]
            {
                new Claim("id", $"{player.Id}"),
                new Claim("name", player.Name)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await this.httpContextAccessor.HttpContext.Authentication.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new GenericPrincipal(identity, null),
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });
        }

        public async Task LogOutAsync()
        {
            await this.httpContextAccessor.HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}