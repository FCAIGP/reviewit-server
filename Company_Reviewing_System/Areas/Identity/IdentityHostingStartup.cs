using System;
using Company_Reviewing_System.Data;
using Company_Reviewing_System.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Company_Reviewing_System.Areas.Identity.IdentityHostingStartup))]
namespace Company_Reviewing_System.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public IConfiguration Configuration { get; }
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                
                services.AddDefaultIdentity<Models.User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<Data.AppDbContext>();

                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                });

                services.AddAuthentication()
.AddGoogle(options =>
{
    IConfigurationSection googleAuthNSection =
                 context.Configuration.GetSection("Authentication:Google");

    options.ClientId = googleAuthNSection["ClientId"];
    options.ClientSecret = googleAuthNSection["ClientSecret"];
});
            });

        }
    }
}