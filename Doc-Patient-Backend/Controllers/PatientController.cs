using Doc_Patient_Backend.Models.DTOs;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Route("api/patient")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/patient/appointments/upcoming
        [HttpGet("appointments/upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _patientService.GetUpcomingAppointmentsAsync(userId);
            return Ok(appointments);
        }

        // GET: api/patient/appointments/completed
        [HttpGet("appointments/completed")]
        public async Task<IActionResult> GetCompletedAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _patientService.GetCompletedAppointmentsAsync(userId);
            return Ok(appointments);
        }

        // GET: api/patient/appointments/all
        [HttpGet("appointments/all")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _patientService.GetAllAppointmentsAsync(userId);
            return Ok(appointments);
        }

        // POST: api/patient/appointments
        [HttpPost("appointments")]
        public async Task<IActionResult> BookAppointment([FromBody] CreateAppointmentDto createAppointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (createdAppointment, errorMessage) = await _patientService.BookAppointmentAsync(userId, createAppointmentDto);

            if (createdAppointment != null)
            {
                // Return 201 Created with the location of the new resource
                // For simplicity, returning the created object directly.
                return CreatedAtAction(nameof(GetUpcomingAppointments), createdAppointment);
            }

            return BadRequest(new { Message = errorMessage });
        }

        // PUT: api/patient/appointments/{id}
        [HttpPut("appointments/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentDto updateAppointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (succeeded, errorMessage) = await _patientService.UpdateAppointmentAsync(userId, id, updateAppointmentDto);

            if (succeeded)
            {
                return Ok(new { Message = "Appointment updated successfully." });
            }

            return BadRequest(new { Message = errorMessage });
        }

        // POST: api/patient/appointments/{id}/cancel
        [HttpPost("appointments/{id}/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (succeeded, errorMessage) = await _patientService.CancelAppointmentAsync(userId, id);

            if (succeeded)
            {
                return Ok(new { Message = "Appointment canceled successfully." });
            }

            return BadRequest(new { Message = errorMessage });
        }
    }
}
