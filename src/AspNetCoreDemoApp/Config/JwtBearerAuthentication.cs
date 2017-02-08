using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreDemoApp.Config
{
    public class JwtBearerAuthentication
    {
        public const string Authorization = "Authorization";
        public const string Issuer = "DemoAppIssuer";
        public const string Audience = "DemoAppAudience";
        public const string Key = "super!secret!key!123";

        public static SymmetricSecurityKey SecurityKey { get; } = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));

        public static JwtBearerOptions Options { get; } = new JwtBearerOptions
        {
            AutomaticAuthenticate = true,
            AutomaticChallenge = true,
            TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                IssuerSigningKey = SecurityKey,
                ValidateIssuerSigningKey = true,
            },
            Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Path.Value == "/connect/token")
                    {
                        return Task.CompletedTask;
                    }

                    var authorizationHeader = context.Request.Headers[Authorization];
                    var queryString = context.Request.QueryString;

                    if (!string.IsNullOrWhiteSpace(authorizationHeader))
                    {
                        return Task.CompletedTask;
                    }

                    if (queryString.HasValue)
                    {
                        var token = GetTokenFromQueryString(queryString.Value);

                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            context.Request.Headers.Add(Authorization, new[] {$"Bearer {token}"});
                            return Task.CompletedTask;
                        }

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.HandleResponse();
                    }

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.HandleResponse();

                    return Task.CompletedTask;
                }
            }
        };

        private static string GetTokenFromQueryString(string queryString)
        {
            const char ParametersDelimiter = '&';

            return queryString
                .TrimStart('?')
                .Split(ParametersDelimiter)
                .SingleOrDefault(x => x.StartsWith("token="))
                ?.Split('=')[1];
        }
    }
}