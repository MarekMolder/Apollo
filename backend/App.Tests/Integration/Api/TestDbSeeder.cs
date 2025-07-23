using System;
using System.Linq;
using System.Threading.Tasks;
using App.DAL.EF;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace App.Tests.Helpers;

public static class TestDbSeeder
{
    public static void SeedAppData(AppDbContext db)
    {
        // lisa vajadusel test andmeid
    }

    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        const string adminEmail = "admin@example.com";
        const string adminPassword = "Admin123!";
        const string adminRole = "admin";

        // Lisa roll kui puudub
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new AppRole
            {
                Id = Guid.NewGuid(),
                Name = adminRole,
                NormalizedName = adminRole.ToUpper()
            });
        }

        // Lisa admin kasutaja kui puudub
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                Email = adminEmail,
                UserName = adminEmail,
                FirstName = "Admin",
                LastName = "User"
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"[ERROR] {error.Code}: {error.Description}");
                }
                throw new ApplicationException("Admin user creation failed: " +
                                               string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // Lisa roll adminile kui puudub
        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}