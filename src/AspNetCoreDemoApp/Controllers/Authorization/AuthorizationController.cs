using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using AspNetCoreDemoApp.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreDemoApp.Controllers.Authorization
{
    [Route("connect")]
    public class AuthorizationController : Controller
    {
        [HttpPost("token")]
        public IActionResult GetJwtToken(string user, string password)
        {
            var userClaims = new Claim[0];

            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }.Union(userClaims);

            var token = new JwtSecurityToken(
                JwtBearerAuthentication.Issuer,
                JwtBearerAuthentication.Audience,
                notBefore: DateTime.Now,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: new SigningCredentials(JwtBearerAuthentication.SecurityKey, SecurityAlgorithms.HmacSha256));

            return this.Json(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}