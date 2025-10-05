using Doc_Patient_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDbContext _context;

        public SettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetAvailableSlotsAsync(DateTime date, string doctorId)
        {
            // This logic can be expanded in the future to fetch working hours per doctor
            var workingHoursStart = new TimeSpan(9, 0, 0); // 9 AM
            var workingHoursEnd = new TimeSpan(17, 0, 0); // 5 PM
            var slotDuration = TimeSpan.FromMinutes(30);

            var appointmentsOnDate = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == date.Date && a.DoctorId == doctorId)
                .Select(a => a.AppointmentTime)
                .ToListAsync();

            var allSlots = new List<string>();
            var currentTime = workingHoursStart;
            while (currentTime < workingHoursEnd)
            {
                allSlots.Add(currentTime.ToString(@"hh\:mm"));
                currentTime = currentTime.Add(slotDuration);
            }

            var availableSlots = allSlots.Except(appointmentsOnDate).ToList();
            return availableSlots;
        }
    }
}