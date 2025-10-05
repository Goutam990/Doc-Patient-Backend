using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAngularApp")] // Make sure this matches your CORS policy in Program.cs
    public class AppointmentController(ApplicationDbContext context) : ControllerBase
    {

        // GET: api/Appointment/GetAllAppointment
        [HttpGet("GetAllAppointment")]
        public IActionResult GetAllAppointment()
        {
            var list = (from appointment in context.Appointments
                        join patient in context.Patients
                        on appointment.PatientId equals patient.PatientId
                        select new
                        {
                            patientName = patient.PatientName,
                            mobileNo = patient.MobileNo,
                            city = patient.City,
                            appointmentDate = appointment.AppointmentDate,
                            isDone = appointment.IsDone
                        }).ToList();

            return Ok(list);
        }

        // GET: api/Appointment/GetDoneAppointment
        [HttpGet("GetDoneAppointment")]
        public IActionResult GetDoneAppointment()
        {
            var list = (from appointment in context.Appointments
                        where appointment.IsDone
                        join patient in context.Patients
                        on appointment.PatientId equals patient.PatientId
                        select new
                        {
                            patientName = patient.PatientName,
                            mobileNo = patient.MobileNo,
                            city = patient.City,
                            appointmentDate = appointment.AppointmentDate,
                            isDone = appointment.IsDone
                        }).ToList();

            return Ok(list);
        }

        // PUT: api/Appointment/{appointmentId}/status
        [HttpPut("{appointmentId}/status")]
        public IActionResult ChangeStatus(int appointmentId, [FromBody] bool isDone)
        {
            var appointment = context.Appointments.SingleOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found" });
            }

            appointment.IsDone = isDone;
            context.SaveChanges();

            return Ok(new { message = "Status Changed Successfully" });
        }

        // POST: api/Appointment/CreateNewAppointment
        [HttpPost("CreateNewAppointment")]
        public IActionResult CreateNewAppointment(NewAppointment obj)
        {
            // Check if patient already exists
            var existingPatient = context.Patients.SingleOrDefault(p => p.MobileNo == obj.MobileNo);

            if (existingPatient == null)
            {
                // Create new patient
                var newPatient = new Patient
                {
                    PatientName = obj.PatientName,
                    Email = obj.Email,
                    MobileNo = obj.MobileNo,
                    City = obj.City,
                    Address = obj.Address
                };

                context.Patients.Add(newPatient);
                context.SaveChanges();
                existingPatient = newPatient;
            }

            // Create new appointment
            var newAppointment = new Appointment
            {
                PatientId = existingPatient.PatientId,
                AppointmentDate = obj.AppointmentDate,
                IsDone = false
            };

            context.Appointments.Add(newAppointment);
            context.SaveChanges();

            return Created("Appointment Created", obj);
        }
    }
}
