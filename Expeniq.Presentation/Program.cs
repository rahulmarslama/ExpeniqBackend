using Expendiq.Domain.Entities.Users;
using Expendiq.Infrastructure.Data;
using Expeniq.Presentation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var app = WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    try
    {
        // Apply pending migrations
        await db.Database.MigrateAsync();
        Console.WriteLine("? Database migrations applied successfully");

        // Seed initial data
        await DataSeeder.SeedAsync(userManager, db);
        Console.WriteLine("? Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"? Error during migration/seeding: {ex.Message}");
        throw;
    }
}

app
    .SetupMiddleware()
    .Run();