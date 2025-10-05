using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAppointmentsAsync()
        {
            return await (from appointment in _context.Appointments
                          join patient in _context.Patients
                          on appointment.PatientId equals patient.PatientId
                          select new
                          {
                              patientName = patient.PatientName,
                              mobileNo = patient.MobileNo,
                              city = patient.City,
                              appointmentDate = appointment.AppointmentDate,
                              isDone = appointment.IsDone
                          }).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetDoneAppointmentsAsync()
        {
            return await (from appointment in _context.Appointments
                          where appointment.IsDone
                          join patient in _context.Patients
                          on appointment.PatientId equals patient.PatientId
                          select new
                          {
                              patientName = patient.PatientName,
                              mobileNo = patient.MobileNo,
                              city = patient.City,
                              appointmentDate = appointment.AppointmentDate,
                              isDone = appointment.IsDone
                          }).ToListAsync();
        }

        public async Task<bool> ChangeAppointmentStatusAsync(int appointmentId, bool isDone)
        {
            var appointment = await _context.Appointments.SingleOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                return false;
            }

            appointment.IsDone = isDone;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Appointment> CreateNewAppointmentAsync(NewAppointment newAppointment)
        {
            var existingPatient = await _context.Patients.SingleOrDefaultAsync(p => p.MobileNo == newAppointment.MobileNo);

            if (existingPatient == null)
            {
                var newPatient = new Patient
                {
                    PatientName = newAppointment.PatientName,
                    Email = newAppointment.Email,
                    MobileNo = newAppointment.MobileNo,
                    City = newAppointment.City,
                    Address = newAppointment.Address
                };
                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();
                existingPatient = newPatient;
            }

            var appointment = new Appointment
            {
                PatientId = existingPatient.PatientId,
                AppointmentDate = newAppointment.AppointmentDate,
                IsDone = false
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}