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
    }
}