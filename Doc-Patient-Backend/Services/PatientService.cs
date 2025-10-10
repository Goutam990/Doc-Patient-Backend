using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(AppointmentDto CreatedAppointment, string ErrorMessage)> BookAppointmentAsync(string patientUserId, CreateAppointmentDto createAppointmentDto)
        {
            // Rule: Patient cannot have multiple active bookings
            var patientInfo = await _context.PatientInfos.FirstOrDefaultAsync(p => p.UserId == patientUserId);
            if (patientInfo == null)
            {
                return (null, "Patient record not found.");
            }

            var hasActiveAppointment = await _context.Appointments
                .AnyAsync(a => a.PatientId == patientInfo.Id && (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed));

            if (hasActiveAppointment)
            {
                return (null, "You already have an active appointment. You cannot book another one.");
            }

            // Rule: No overlapping appointments for the single doctor
            var proposedStartTime = createAppointmentDto.StartTime;
            var proposedEndTime = proposedStartTime.AddHours(1);

            var isSlotTaken = await _context.Appointments
                .AnyAsync(a => (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed) &&
                               (proposedStartTime < a.EndTime && proposedEndTime > a.StartTime));

            if (isSlotTaken)
            {
                return (null, "The requested time slot is no longer available.");
            }

            var newAppointment = new Appointment
            {
                PatientId = patientInfo.Id,
                StartTime = proposedStartTime,
                EndTime = proposedEndTime
                // Status defaults to Pending
            };

            await _context.Appointments.AddAsync(newAppointment);

            // Log the creation
            var log = new AppointmentLog
            {
                Appointment = newAppointment, // EF will link the ID upon saving
                ActionType = ActionType.Created,
                PerformedBy = PerformedBy.Patient
            };
            await _context.AppointmentLogs.AddAsync(log);

            await _context.SaveChangesAsync();

            var appointmentDto = new AppointmentDto
            {
                Id = newAppointment.Id,
                StartTime = newAppointment.StartTime,
                EndTime = newAppointment.EndTime,
                Status = newAppointment.Status.ToString(),
                IsRevisit = newAppointment.IsRevisit
            };

            return (appointmentDto, null);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> CancelAppointmentAsync(string patientUserId, int appointmentId)
        {
            var patientInfo = await _context.PatientInfos.FirstOrDefaultAsync(p => p.UserId == patientUserId);
            var appointment = await _context.Appointments.FindAsync(appointmentId);

            if (appointment == null || patientInfo == null || appointment.PatientId != patientInfo.Id)
            {
                return (false, "Appointment not found or you do not have permission to cancel it.");
            }

            // Rule: Can only cancel pending or approved appointments
            if (appointment.Status != AppointmentStatus.Pending && appointment.Status != AppointmentStatus.Confirmed)
            {
                return (false, $"Cannot cancel an appointment with status '{appointment.Status}'.");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;

            // Log the cancellation
            var log = new AppointmentLog
            {
                AppointmentId = appointmentId,
                ActionType = ActionType.Cancelled,
                PerformedBy = PerformedBy.Patient
            };
            await _context.AppointmentLogs.AddAsync(log);

            await _context.SaveChangesAsync();
            // TODO: Add refund logic if payment was made
            return (true, null);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(string patientUserId)
        {
            var patientInfo = await _context.PatientInfos.FirstOrDefaultAsync(p => p.UserId == patientUserId);
            if (patientInfo == null) return new List<AppointmentDto>();

            return await _context.Appointments
                .Where(a => a.PatientId == patientInfo.Id)
                .OrderByDescending(a => a.StartTime)
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

        public async Task<IEnumerable<AppointmentDto>> GetCompletedAppointmentsAsync(string patientUserId)
        {
            var patientInfo = await _context.PatientInfos.FirstOrDefaultAsync(p => p.UserId == patientUserId);
            if (patientInfo == null) return new List<AppointmentDto>();

            return await _context.Appointments
                .Where(a => a.PatientId == patientInfo.Id && a.Status == AppointmentStatus.Completed)
                .OrderByDescending(a => a.StartTime)
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

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(string patientUserId)
        {
            var patientInfo = await _context.PatientInfos.FirstOrDefaultAsync(p => p.UserId == patientUserId);
            if (patientInfo == null) return new List<AppointmentDto>();

            return await _context.Appointments
                .Where(a => a.PatientId == patientInfo.Id && (a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed))
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

        public async Task<(bool Succeeded, string ErrorMessage)> UpdateAppointmentAsync(string patientUserId, int appointmentId, UpdateAppointmentDto updateAppointmentDto)
        {
            var patientInfo = await _context.PatientInfos.FirstOrDefaultAsync(p => p.UserId == patientUserId);
            var appointment = await _context.Appointments.FindAsync(appointmentId);

            if (appointment == null || patientInfo == null || appointment.PatientId != patientInfo.Id)
            {
                return (false, "Appointment not found or you do not have permission to edit it.");
            }

            // Rule: Can only edit before the scheduled time
            if (appointment.StartTime <= DateTime.UtcNow)
            {
                return (false, "Cannot edit an appointment that is already in the past.");
            }

            var oldTime = appointment.StartTime;
            var newTime = updateAppointmentDto.StartTime;

            appointment.StartTime = newTime;
            appointment.EndTime = newTime.AddHours(1);
            appointment.UpdatedAt = DateTime.UtcNow;

            // Log the update
            var log = new AppointmentLog
            {
                AppointmentId = appointmentId,
                ActionType = ActionType.Edited,
                PerformedBy = PerformedBy.Patient,
                OldTime = oldTime,
                NewTime = newTime
            };
            await _context.AppointmentLogs.AddAsync(log);

            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}
