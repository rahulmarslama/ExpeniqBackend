using Expendiq.Domain.Entities.Users;
using Expendiq.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Expeniq.Presentation
{
    public class DataSeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            await SeedUsersAsync(userManager);
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var adminExists = await userManager.FindByNameAsync("admin");
            if (adminExists != null)
                return;

            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@expendiq.com",
                FullName = "Admin",
                MobileNumber = "123456789",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, "Admin@123456");
        }

    }
}
