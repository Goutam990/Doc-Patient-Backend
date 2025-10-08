using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Models.DTOs;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IAppointmentService appointmentService) : ControllerBase
    {
        // GET: api/Appointment
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var list = await appointmentService.GetAllAppointmentsAsync();
            return Ok(list);
        }

        // GET: api/appointments/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> GetPatientAppointments(string patientId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(UserRoles.Admin);

            // A patient can only view their own appointments. An admin can view any patient's appointments.
            if (!isAdmin && loggedInUserId != patientId)
            {
                return Forbid();
            }

            var appointments = await appointmentService.GetUpcomingAppointmentsForPatientAsync(patientId);
            return Ok(appointments);
        }

        // PATCH: api/Appointment/{id}/status
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Doctor")] // Only Admin/Doctor can change status
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status cannot be empty.");
            }

            var result = await appointmentService.ChangeAppointmentStatusAsync(id, status);
            if (!result)
            {
                return NotFound(new { message = "Appointment not found" });
            }
            return Ok(new { message = "Status Changed Successfully" });
        }

        // POST: api/appointments/book
        [HttpPost("book")]
        [Authorize(Roles = UserRoles.Patient)]
        public async Task<IActionResult> BookAppointment([FromBody] CreateAppointmentDto obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInPatientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (createdAppointment, error) = await appointmentService.CreateNewAppointmentAsync(obj, loggedInPatientId);

            if (error != null)
            {
                return BadRequest(new { message = error });
            }

            var appointmentDto = new AppointmentDto
            {
                Id = createdAppointment.Id,
                PatientName = createdAppointment.PatientName,
                Age = createdAppointment.Age,
                Gender = createdAppointment.Gender,
                StartTime = createdAppointment.StartTime,
                EndTime = createdAppointment.EndTime,
                PhoneNumber = createdAppointment.PhoneNumber,
                Address = createdAppointment.Address,
                Status = createdAppointment.Status,
                PatientId = createdAppointment.PatientId,
                DoctorId = createdAppointment.DoctorId
            };

            return CreatedAtAction(nameof(GetAllAppointments), new { id = appointmentDto.Id }, appointmentDto);
        }

        // PATCH: api/appointments/{id}/cancel
        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = UserRoles.Patient)]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (success, error) = await appointmentService.CancelAppointmentAsync(id, patientId);

            if (!success)
            {
                return BadRequest(new { message = error });
            }

            return NoContent();
        }
    }
}