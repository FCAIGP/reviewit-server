using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ReviewItServer.Models;
using ReviewItServer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Data
{
    public class SeedDb
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.EnsureCreated();

            var roleCheck = await roleManager.RoleExistsAsync(Roles.Admin);
            if (!roleCheck)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            var user = await userManager.FindByEmailAsync("test@test.com");
            if(user == null) {
                user = new User()
                {
                    FirstName = "Alice",
                    LastName = "Bob",
                    Email = "test@test.com",
                    UserName = "test",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                await userManager.CreateAsync(user, "test");
               
            }
            var admin = await userManager.FindByEmailAsync("admin@admin.com");
            if(admin == null)
            {
                admin = new User()
                {
                    FirstName = "Super",
                    LastName = "Man",
                    Email = "admin@admin.com",
                    UserName = "admin",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                await userManager.CreateAsync(user, "admin");
            }
            await userManager.AddToRoleAsync(admin, Roles.Admin);

        }
    }
}
