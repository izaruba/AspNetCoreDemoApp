using AspNetCoreDemoApp.Gameplay;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreDemoApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            }

            this.configuration = builder.Build();
        }

        private readonly IConfigurationRoot configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IWordsRepository>(new NonRecurringWordsRepository(new [] { "programming", "developer", "code" }));
            services.AddSingleton<GameServer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<AuthenticationService>();

            services
                .AddMvc()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseWamp();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                CookieName = "AUTH_COOKIE",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseMvc();
        }
    }
}