using App.DAL.EF;
using App.Domain.Identity;
using App.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace App.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Eemalda olemasolevad AppDbContext konfiguratsioonid
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

            // 2. Lisa uus PostgreSQL konfiguratsioon testandmebaasile
            var connectionString = $"Host=localhost;Port=5432;Database=TEST_{Guid.NewGuid()};Username=postgres;Password=postgres";
            services.AddDbContext<AppDbContext>(options =>
                options
                    .UseNpgsql(connectionString, o =>
                        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            );

            // 3. Rakenda migratsioonid ja tee seeding
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
            var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scopedServices.GetRequiredService<RoleManager<AppRole>>();

            try
            {
                db.Database.EnsureDeleted();
                db.Database.Migrate();

                // 4. Seedi rollid ja admin-kasutaja
                TestDbSeeder.SeedAppData(db); // kui sul on mingi app-specific seeding
                TestDbSeeder.SeedAdminUserAsync(scopedServices).Wait(); // loob admin@example.com kasutaja
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Test DB seeding failed: {Message}", ex.Message);
            }
        });
    }
}