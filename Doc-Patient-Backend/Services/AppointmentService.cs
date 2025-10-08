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
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISettingsService _settingsService;
        private readonly IPaymentService _paymentService;

        public AppointmentService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISettingsService settingsService,
            IPaymentService paymentService)
        {
            _context = context;
            _userManager = userManager;
            _settingsService = settingsService;
            _paymentService = paymentService;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientName = a.PatientName,
                    Age = a.Age,
                    Gender = a.Gender,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    Status = a.Status,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsForPatientAsync(string patientId)
        {
            var today = DateTime.UtcNow;
            return await _context.Appointments
                .Where(a => a.PatientId == patientId && a.StartTime >= today)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientName = a.PatientName,
                    Age = a.Age,
                    Gender = a.Gender,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    PhoneNumber = a.PhoneNumber,
                    Address = a.Address,
                    Status = a.Status,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId
                })
                .ToListAsync();
        }

        public async Task<bool> ChangeAppointmentStatusAsync(int appointmentId, string status)
        {
            var appointment = await _context.Appointments.SingleOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null) return false;

            appointment.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(Appointment, string Error)> CreateNewAppointmentAsync(CreateAppointmentDto createAppointmentDto, string patientId)
        {
            try
            {
                // Rule: Patient can only book up to 2 months in advance
                if (createAppointmentDto.StartTime > DateTime.UtcNow.AddMonths(2))
                {
                    return (null, "Appointments can only be booked up to two months in advance.");
                }

                // Rule: Check if the slot is actually available
                var availableSlots = await _settingsService.GetAvailableSlotsAsync(createAppointmentDto.StartTime.Date, createAppointmentDto.DoctorId);
                if (!availableSlots.Contains(createAppointmentDto.StartTime.ToString("HH:mm")))
                {
                    return (null, "The selected time slot is not available.");
                }

                var patient = await _userManager.FindByIdAsync(patientId);
                if (patient == null) return (null, "Patient not found.");

                var appointment = new Appointment
                {
                    PatientName = $"{patient.FirstName} {patient.LastName}",
                    Age = createAppointmentDto.Age,
                    Gender = createAppointmentDto.Gender,
                    StartTime = createAppointmentDto.StartTime,
                    EndTime = createAppointmentDto.StartTime.AddHours(1), // Rule: 1-hour slot
                    PhoneNumber = createAppointmentDto.PhoneNumber,
                    Address = createAppointmentDto.Address,
                    PatientId = patientId,
                    DoctorId = createAppointmentDto.DoctorId,
                    Status = "Pending",
                    PaymentIntentId = createAppointmentDto.PaymentIntentId
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return (appointment, null);
            }
            catch (Exception ex)
            {
                // Log the exception ex
                return (null, "An unexpected error occurred while booking the appointment.");
            }
        }

        public async Task<(bool, string Error)> CancelAppointmentAsync(int appointmentId, string patientId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);

            if (appointment == null)
            {
                return (false, "Appointment not found.");
            }

            if (appointment.PatientId != patientId)
            {
                return (false, "You do not have permission to cancel this appointment.");
            }

            if (appointment.StartTime < DateTime.UtcNow)
            {
                return (false, "Cannot cancel an appointment that has already passed.");
            }

            if (appointment.Status != "Pending" && appointment.Status != "Confirmed")
            {
                return (false, $"Cannot cancel an appointment with status '{appointment.Status}'.");
            }

            // Process refund if a payment was made
            if (!string.IsNullOrEmpty(appointment.PaymentIntentId))
            {
                var refund = await _paymentService.ProcessRefundAsync(appointment.PaymentIntentId);
                if (refund == null || refund.Status == "failed")
                {
                    return (false, "Refund processing failed. Please contact support.");
                }
            }

            appointment.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}