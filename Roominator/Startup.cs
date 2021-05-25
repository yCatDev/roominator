using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Roominator.Data;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Roominator
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddServerSideBlazor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                 AddFacebook(facebookoptions =>
                {
                    facebookoptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookoptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                }
            ).
                AddGoogle(googleoptions => 
                {
                    googleoptions.ClientId = Configuration["Authentication:Google:ClientId"];
                    googleoptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                }).AddCookie(options =>
                {
                    options.CookieManager = new ChunkingCookieManager();

                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            services.AddSingleton<WeatherForecastService>();
            services.AddServerSideBlazor().AddCircuitOptions(options => {
                if (_env.IsDevelopment())
                {
                    options.DetailedErrors = true;
                }
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.UseHttpsRedirection();
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Remove(".data");
            provider.Mappings[".data"] = "application/octet-stream";
            provider.Mappings.Remove(".wasm");
            provider.Mappings[".wasm"] = "application/wasm";
            provider.Mappings.Remove(".symbols.json");
            provider.Mappings[".symbols.json"] = "application/octet-stream";
            app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                // Needed because of mixing http and https.
                RequireHeaderSymmetry = false,
            };

            // Accept X-Forwarded-* headers from all sources.
            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardOptions);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
