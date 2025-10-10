using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<(bool Succeeded, string ErrorMessage)> ApproveAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return (false, "Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Pending)
            {
                return (false, "Only pending appointments can be approved.");
            }

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.UpdatedAt = DateTime.UtcNow;

            // Log the action
            var log = new AppointmentLog
            {
                AppointmentId = appointmentId,
                ActionType = ActionType.Approved,
                PerformedBy = PerformedBy.Doctor
            };
            await _context.AppointmentLogs.AddAsync(log);

            await _context.SaveChangesAsync();
            // TODO: Add patient notification logic here
            return (true, null);
        }

        public async Task<IEnumerable<PatientDetailsDto>> GetAllPatientsAsync()
        {
            return await _context.PatientInfos
                .Include(pi => pi.User)
                .Select(pi => new PatientDetailsDto
                {
                    Id = pi.Id,
                    UserId = pi.UserId,
                    FirstName = pi.FirstName,
                    LastName = pi.LastName,
                    Email = pi.User.Email,
                    DateOfBirth = pi.DateOfBirth,
                    Gender = pi.Gender,
                    CountryCode = pi.CountryCode,
                    PhoneNumber = pi.PhoneNumber,
                    IllnessHistory = pi.IllnessHistory,
                    Picture = pi.Picture
                })
                .ToListAsync();
        }

        public async Task<PatientDetailsDto> GetPatientDetailsAsync(int patientInfoId)
        {
            return await _context.PatientInfos
                .Where(pi => pi.Id == patientInfoId)
                .Include(pi => pi.User)
                .Select(pi => new PatientDetailsDto
                {
                    Id = pi.Id,
                    UserId = pi.UserId,
                    FirstName = pi.FirstName,
                    LastName = pi.LastName,
                    Email = pi.User.Email,
                    DateOfBirth = pi.DateOfBirth,
                    Gender = pi.Gender,
                    CountryCode = pi.CountryCode,
                    PhoneNumber = pi.PhoneNumber,
                    IllnessHistory = pi.IllnessHistory,
                    Picture = pi.Picture
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(string doctorId)
        {
            var today = DateTime.UtcNow;
            // Since there's only one doctor, we don't strictly need the doctorId, but it's good practice
            return await _context.Appointments
                .Where(a => (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed) && a.StartTime >= today)
                .OrderBy(a => a.StartTime)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status.ToString(),
                    IsRevisit = a.IsRevisit
                })
                .ToListAsync();
        }

        public async Task<(bool Succeeded, string ErrorMessage)> RejectAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return (false, "Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Pending)
            {
                return (false, "Only pending appointments can be rejected.");
            }

            appointment.Status = AppointmentStatus.Rejected;
            appointment.UpdatedAt = DateTime.UtcNow;

            // Log the action
            var log = new AppointmentLog
            {
                AppointmentId = appointmentId,
                ActionType = ActionType.Rejected,
                PerformedBy = PerformedBy.Doctor,
                Reason = "Rejected by doctor." // A default reason
            };
            await _context.AppointmentLogs.AddAsync(log);

            await _context.SaveChangesAsync();
            // TODO: Add patient notification logic here
            return (true, null);
        }

        public async Task<(AppointmentDto CreatedRevisit, string ErrorMessage)> ScheduleRevisitAsync(int originalAppointmentId)
        {
            var originalAppointment = await _context.Appointments.FindAsync(originalAppointmentId);
            if (originalAppointment == null)
            {
                return (null, "Original appointment not found.");
            }

            // A revisit can only be scheduled for a completed or approved appointment
            if (originalAppointment.Status != AppointmentStatus.Completed && originalAppointment.Status != AppointmentStatus.Confirmed)
            {
                return (null, "Revisit can only be scheduled for a completed or confirmed appointment.");
            }

            // For simplicity, let's schedule the revisit for 7 days after the original appointment
            var revisitStartTime = originalAppointment.StartTime.AddDays(7);

            var newAppointment = new Appointment
            {
                PatientId = originalAppointment.PatientId,
                StartTime = revisitStartTime,
                EndTime = revisitStartTime.AddHours(1),
                Status = AppointmentStatus.Confirmed, // Revisits are auto-confirmed
                IsRevisit = true
            };

            await _context.Appointments.AddAsync(newAppointment);

            // Log the action
            var log = new AppointmentLog
            {
                AppointmentId = newAppointment.Id,
                ActionType = ActionType.Created,
                PerformedBy = PerformedBy.Doctor,
                Reason = $"Revisit scheduled for original appointment ID: {originalAppointmentId}"
            };
            await _context.AppointmentLogs.AddAsync(log);

            await _context.SaveChangesAsync();

            var revisitDto = new AppointmentDto
            {
                Id = newAppointment.Id,
                StartTime = newAppointment.StartTime,
                EndTime = newAppointment.EndTime,
                Status = newAppointment.Status.ToString(),
                IsRevisit = newAppointment.IsRevisit
            };

            return (revisitDto, null);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> UpdateAvailabilityAsync(string doctorId, UpdateAvailabilityDto updateAvailabilityDto)
        {
            var availability = await _context.AvailabilityHours.FirstOrDefaultAsync(a => a.DoctorId == doctorId);
            if (availability == null)
            {
                availability = new AvailabilityHour
                {
                    DoctorId = doctorId,
                    StartTime = updateAvailabilityDto.StartTime,
                    EndTime = updateAvailabilityDto.EndTime
                };
                await _context.AvailabilityHours.AddAsync(availability);
            }
            else
            {
                availability.StartTime = updateAvailabilityDto.StartTime;
                availability.EndTime = updateAvailabilityDto.EndTime;
                availability.UpdatedAt = DateTime.UtcNow;
            }

            // Auto-cancel logic
            var appointmentsToCancel = await _context.Appointments
                .Where(a => a.Status == AppointmentStatus.Confirmed &&
                            (a.StartTime < availability.StartTime || a.EndTime > availability.EndTime))
                .ToListAsync();

            foreach (var appointment in appointmentsToCancel)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.UpdatedAt = DateTime.UtcNow;

                var log = new AppointmentLog
                {
                    AppointmentId = appointment.Id,
                    ActionType = ActionType.AutoCancelled,
                    PerformedBy = PerformedBy.System,
                    Reason = "Appointment falls outside the doctor's new availability hours."
                };
                await _context.AppointmentLogs.AddAsync(log);
                // TODO: Add refund logic and patient notification logic here
            }

            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}
