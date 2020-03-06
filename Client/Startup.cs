using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => 
            {
                // We check the cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                // When we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                //use this to check if we are allowd to do something
                config.DefaultChallengeScheme = "OurServer";
            }).AddCookie("ClientCookie")
            .AddOAuth("OurServer", config => 
            {
                config.AuthorizationEndpoint = "http://localhost:5000/oauth/authorize";
                config.TokenEndpoint = "http://localhost:5000/oauth/token";
                config.ClientId = "client_id";
                config.ClientSecret = "client_secret";
                config.CallbackPath = "/oauth/callback";
                config.SaveTokens = true;
                config.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var accessToken = context.AccessToken;
                        var base64Payload = accessToken.Split('.')[1];
                        base64Payload = base64Payload.PadRight(base64Payload.Length + (4 - base64Payload.Length % 4) % 4, '=');
                        var bytes = Convert.FromBase64String(base64Payload);
                        var jsonPayload = Encoding.UTF8.GetString(bytes);
                        var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);
                        foreach (var claim in claims)
                        {
                            context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddHttpClient();
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            // who are you?
            app.UseAuthentication();
            // are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
