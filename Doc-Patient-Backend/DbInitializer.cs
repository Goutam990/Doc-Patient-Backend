using Doc_Patient_Backend.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Doc_Patient_Backend
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(UserRoles.Doctor))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));
            }
            if (!await roleManager.RoleExistsAsync(UserRoles.Patient))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Patient));
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }
            }
        }
    }
}