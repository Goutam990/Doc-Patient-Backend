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

        public PatientsService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(IEnumerable<ApplicationUser> Patients, int TotalCount)> GetPatientsAsync(int page, int pageSize)
        {
            var query = _userManager.Users.Where(u => u.Role == UserRoles.Patient);

            var totalPatients = await query.CountAsync();
            var paginatedPatients = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (paginatedPatients, totalPatients);
        }

        public async Task<ApplicationUser> ToggleBlockPatientAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.Role != UserRoles.Patient)
            {
                return null;
            }

            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return user;
        }
    }
}