using Microsoft.AspNetCore.Builder;
using WampSharp.AspNetCore.WebSockets.Server;
using WampSharp.Binding;
using WampSharp.V2;

namespace AspNetCoreDemoApp
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWamp(this IApplicationBuilder app)
        {
            var host = new WampHost();

            app.Map("/ws", builder =>
            {
                builder.UseWebSockets(new WebSocketOptions
                {
                    ReplaceFeature = true
                });

                host.RegisterTransport(new AspNetCoreWebSocketTransport(builder),
                                       new JTokenJsonBinding());
            });

            host.Open();

            return app;
        }
    }
}