using Doc_Patient_Backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Doc_Patient_Backend
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            Console.WriteLine("Seeding roles...");
            try
            {
                // Ensure the core roles exist
                if (!await roleManager.RoleExistsAsync(UserRoles.Doctor))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));
                    Console.WriteLine("Doctor role created.");
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.Patient))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Patient));
                    Console.WriteLine("Patient role created.");
                }
                Console.WriteLine("Role seeding finished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding roles: {ex.Message}");
            }
        }

        // WARNING: This method uses a hardcoded password and is intended for development purposes only.
        public static async Task SeedDoctorUserAsync(UserManager<ApplicationUser> userManager)
        {
            Console.WriteLine("Seeding doctor user...");
            try
            {
                if (await userManager.FindByEmailAsync("doctor@demo.com") == null)
                {
                    var doctorUser = new ApplicationUser
                    {
                        UserName = "doctor@demo.com",
                        Email = "doctor@demo.com",
                        FirstName = "Demo",
                        LastName = "Doctor",
                        EmailConfirmed = true
                    };

                    // Use the password specified in the requirements
                    var result = await userManager.CreateAsync(doctorUser, "Admin@12345");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(doctorUser, UserRoles.Doctor);
                        Console.WriteLine("Doctor user (doctor@demo.com) created and assigned to Doctor role.");
                    }
                    else
                    {
                        Console.WriteLine("Doctor user creation failed:");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"- {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Doctor user (doctor@demo.com) already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the doctor user: {ex.Message}");
            }
        }
    }
}