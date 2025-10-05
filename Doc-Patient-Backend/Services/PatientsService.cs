using Doc_Patient_Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class PatientsService : IPatientsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PatientsService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<(IEnumerable<ApplicationUser> Patients, int TotalCount)> GetPatientsAsync(int page, int pageSize)
        {
            var query = from user in _context.Users
                        join userRole in _context.UserRoles on user.Id equals userRole.UserId
                        join role in _context.Roles on userRole.RoleId equals role.Id
                        where role.Name == UserRoles.Patient
                        select user;

            var totalCount = await query.CountAsync();
            var patients = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (patients, totalCount);
        }

        public async Task<ApplicationUser> ToggleBlockPatientAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, UserRoles.Patient))
            {
                return null;
            }

            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return user;
        }
    }
}