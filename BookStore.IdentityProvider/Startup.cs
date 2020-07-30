using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore.IdentityProvider
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
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(Configuration.GetSection("IdentityServer:IdentityResources"))
                .AddInMemoryApiResources(Configuration.GetSection("IdentityServer:ApiResources"))
                .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()));
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
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
