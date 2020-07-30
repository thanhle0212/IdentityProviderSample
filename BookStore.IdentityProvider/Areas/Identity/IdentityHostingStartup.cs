using System;
using BookStore.IdentityProvider.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BookStore.IdentityProvider.Areas.Identity.IdentityHostingStartup))]
namespace BookStore.IdentityProvider.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BookStoreIdentityProviderContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("BookStoreIdentityProviderContextConnection")));

                services.AddIdentity<ApplicationUser, IdentityRole > (options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<BookStoreIdentityProviderContext>().AddDefaultUI()
                    .AddDefaultTokenProviders();
                services.AddAuthentication().
              AddGoogle(o => {
                  o.ClientId = context.Configuration["GoogleAuthentication:ClientId"];
                  o.ClientSecret = context.Configuration["GoogleAuthentication:ClientSecret"];
              }
              );
                services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
                services.AddTransient<IEmailSender, CustomEmailSender>();
            });
        }
    }
}