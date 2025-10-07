using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
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
            var today = DateTime.UtcNow.Date;
            return await _context.Appointments
                .Where(a => a.PatientId == patientId && a.AppointmentDate >= today)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    PatientName = a.PatientName,
                    Age = a.Age,
                    Gender = a.Gender,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
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
            if (appointment == null)
            {
                return false;
            }

            appointment.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Appointment> CreateNewAppointmentAsync(CreateAppointmentDto createAppointmentDto)
        {
            var patient = await _userManager.FindByIdAsync(createAppointmentDto.PatientId);
            var doctor = await _userManager.FindByIdAsync(createAppointmentDto.DoctorId);

            if (patient == null || doctor == null)
            {
                // Or throw a custom exception
                return null;
            }

            // Calculate Start and End time
            var appointmentDateTime = createAppointmentDto.AppointmentDate.Date;
            if (TimeSpan.TryParse(createAppointmentDto.AppointmentTime, out var time))
            {
                appointmentDateTime = appointmentDateTime.Add(time);
            }

            var appointment = new Appointment
            {
                PatientName = createAppointmentDto.PatientName,
                Age = createAppointmentDto.Age,
                Gender = createAppointmentDto.Gender,
                AppointmentDate = appointmentDateTime,
                AppointmentTime = createAppointmentDto.AppointmentTime,
                EndTime = appointmentDateTime.AddHours(1),
                PhoneNumber = createAppointmentDto.PhoneNumber,
                Address = createAppointmentDto.Address,
                PatientId = createAppointmentDto.PatientId,
                DoctorId = createAppointmentDto.DoctorId,
                Status = "Scheduled"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}