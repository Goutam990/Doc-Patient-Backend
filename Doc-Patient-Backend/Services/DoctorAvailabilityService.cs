using Doc_Patient_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly ApplicationDbContext _context;

        public DoctorAvailabilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorAvailability> AddVacationDayAsync(string doctorId, DateTime vacationDate, string reason)
        {
            var availability = new DoctorAvailability
            {
                DoctorId = doctorId,
                VacationDate = vacationDate.Date,
                Reason = reason
            };

            _context.DoctorAvailabilities.Add(availability);
            await _context.SaveChangesAsync();
            return availability;
        }

        public async Task<IEnumerable<DoctorAvailability>> GetVacationDaysAsync(string doctorId)
        {
            return await _context.DoctorAvailabilities
                .Where(da => da.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<bool> DeleteVacationDayAsync(int availabilityId, string doctorId)
        {
            var availability = await _context.DoctorAvailabilities
                .FirstOrDefaultAsync(da => da.Id == availabilityId && da.DoctorId == doctorId);

            if (availability == null)
            {
                return false;
            }

            _context.DoctorAvailabilities.Remove(availability);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}