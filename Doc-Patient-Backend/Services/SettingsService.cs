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
            // Check if the doctor is on vacation on the selected date
            var isVacation = await _context.DoctorAvailabilities
                .AnyAsync(da => da.DoctorId == doctorId && da.VacationDate.Date == date.Date);

            if (isVacation)
            {
                return new List<string>(); // Return an empty list if the doctor is on vacation
            }

            // Define doctor's working hours and slot duration (can be moved to config or doctor-specific settings later)
            var workingHoursStart = new TimeSpan(10, 0, 0); // 10 AM
            var workingHoursEnd = new TimeSpan(18, 0, 0);   // 6 PM
            var slotDuration = TimeSpan.FromHours(1);

            // Get all existing appointments for the specified doctor on that day
            var existingAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.StartTime.Date == date.Date)
                .Select(a => a.StartTime)
                .ToListAsync();

            // Generate all possible time slots for the day
            var allSlots = new List<DateTime>();
            var currentTime = date.Date.Add(workingHoursStart);
            var endTime = date.Date.Add(workingHoursEnd);

            while (currentTime < endTime)
            {
                allSlots.Add(currentTime);
                currentTime = currentTime.Add(slotDuration);
            }

            // Filter out the time slots that are already booked
            var availableSlots = allSlots
                .Where(slot => !existingAppointments.Any(booked => booked.TimeOfDay == slot.TimeOfDay))
                .Select(slot => slot.ToString("HH:mm"))
                .ToList();

            return availableSlots;
        }
    }
}