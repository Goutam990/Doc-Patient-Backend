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
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    Console.WriteLine("Admin role created.");
                }
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

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            Console.WriteLine("Seeding admin user...");
            try
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
                        Console.WriteLine("Admin user created and assigned to Admin role.");
                    }
                    else
                    {
                        Console.WriteLine("Admin user creation failed:");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"- {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Admin user already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the admin user: {ex.Message}");
            }
        }

        public static async Task SeedDoctorUserAsync(UserManager<ApplicationUser> userManager)
        {
            Console.WriteLine("Seeding doctor user...");
            try
            {
                if (await userManager.FindByEmailAsync("doctor@example.com") == null)
                {
                    var doctorUser = new ApplicationUser
                    {
                        UserName = "doctor@example.com",
                        Email = "doctor@example.com",
                        FirstName = "Default",
                        LastName = "Doctor",
                        Specialization = "General Medicine",
                        Experience = 5,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(doctorUser, "Doctor@123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(doctorUser, UserRoles.Doctor);
                        Console.WriteLine("Doctor user created and assigned to Doctor role.");
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
                    Console.WriteLine("Doctor user already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the doctor user: {ex.Message}");
            }
        }
    }
}