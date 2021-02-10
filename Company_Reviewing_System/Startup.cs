using Company_Reviewing_System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Company_Reviewing_System.Models;
using Company_Reviewing_System.Utility;



namespace Company_Reviewing_System
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

            services.AddDbContext<Data.AppDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            /* leave it for belal's configuration
            services.AddDbContext<Data.AppDbContext>(options =>
             options.UseLazyLoadingProxies().UseSqlServer(
            Configuration.GetConnectionString("Belal")));*/

            services.AddControllersWithViews().AddRazorRuntimeCompilation();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            
            CreateUserRoles(services).Wait();
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            IdentityResult roleResult;
            var roleCheck = await RoleManager.RoleExistsAsync(Roles.Admin);
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            User user = await UserManager.FindByEmailAsync("admin@admin.com");
            if (user != null) await UserManager.AddToRoleAsync(user, Roles.Admin);
        }
    }
}

