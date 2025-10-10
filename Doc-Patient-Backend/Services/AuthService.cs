using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Doc_Patient_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<(string Token, string ErrorMessage)> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return (null, "Invalid email or password.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), null);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> RegisterAsync(RegisterDto registerDto)
        {
            // Remote validation check
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return (false, "An account with this email already exists.");
            }

            if (await _context.PatientInfos.AnyAsync(p => p.PhoneNumber == registerDto.PhoneNumber))
            {
                return (false, "An account with this phone number already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                // Aggregate errors
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"User creation failed: {errors}");
            }

            // Add user to patient role
            await _userManager.AddToRoleAsync(user, UserRoles.Patient);

            // Create PatientInfo record
            var patientInfo = new PatientInfo
            {
                UserId = user.Id,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DateOfBirth = registerDto.DateOfBirth,
                Gender = registerDto.Gender,
                CountryCode = registerDto.CountryCode,
                PhoneNumber = registerDto.PhoneNumber,
                IllnessHistory = registerDto.IllnessHistory,
                Picture = registerDto.Picture
            };

            await _context.PatientInfos.AddAsync(patientInfo);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
