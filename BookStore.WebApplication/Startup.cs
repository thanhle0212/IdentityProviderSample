using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BookStore.WebApplication.AuthorizationHelpers;
using BookStore.WebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(o => o.Filters.Add(new AuthorizeFilter()));
            services.AddHttpContextAccessor();
            services.AddAuthentication(o => {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddOpenIdConnect(options => {
                    options.Authority = "https://localhost:44380";
                    options.ClientId = "bookstore_webapp";
                    options.ClientSecret = "supersecret";
                    options.CallbackPath = "/signin-oidc";

                    options.Scope.Add("openid");
                    options.Scope.Add("bookstore");
                    options.Scope.Add("bookstore_apis");
                    options.Scope.Add("bookstore_viewbook");
                    //options.Scope.Add("offline_access");

                    options.SaveTokens = true;
                    options.ResponseType = "code";
                    options.ResponseMode = "form_post";

                    options.UsePkce = true;
                });

                services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()));

            services.AddHttpContextAccessor();
            services.AddHttpClient<IBookStoreAPIService, BookStoreAPIService>(
                async (c, client) =>
                {
                    var accessor = c.GetRequiredService<IHttpContextAccessor>();
                    var accessToken = await accessor.HttpContext.GetTokenAsync("access_token");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.BaseAddress = new Uri("https://localhost:44308/api/");
                });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("CanViewBook", policy => policy.RequireRole("Administrator" , "View"));

                options.AddPolicy("CanAddBook", policy => policy.RequireClaim("Permission", "AddBook"));
                options.AddPolicy("StartedYear", policy => policy.AddRequirements(new StartedYearRequirement(2010)));
            });
            services.AddSingleton<IAuthorizationHandler, StartedYearAuthorizationHandler>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
