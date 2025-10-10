using Doc_Patient_Backend.Models.DTOs;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Route("api/doctor")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: api/doctor/patients
        [HttpGet("patients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _doctorService.GetAllPatientsAsync();
            return Ok(patients);
        }

        // GET: api/doctor/patients/{id}
        [HttpGet("patients/{id}")]
        public async Task<IActionResult> GetPatientDetails(int id)
        {
            var patient = await _doctorService.GetPatientDetailsAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        // GET: api/doctor/appointments/upcoming
        [HttpGet("appointments/upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _doctorService.GetUpcomingAppointmentsAsync(doctorId);
            return Ok(appointments);
        }

        // POST: api/doctor/availability
        [HttpPost("availability")]
        public async Task<IActionResult> UpdateAvailability([FromBody] UpdateAvailabilityDto updateAvailabilityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (succeeded, errorMessage) = await _doctorService.UpdateAvailabilityAsync(doctorId, updateAvailabilityDto);

            if (succeeded)
            {
                return Ok(new { Message = "Availability updated successfully." });
            }

            return BadRequest(new { Message = errorMessage });
        }

        // POST: api/doctor/appointments/{id}/approve
        [HttpPost("appointments/{id}/approve")]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var (succeeded, errorMessage) = await _doctorService.ApproveAppointmentAsync(id);
            if (succeeded)
            {
                return Ok(new { Message = "Appointment approved successfully." });
            }
            return BadRequest(new { Message = errorMessage });
        }

        // POST: api/doctor/appointments/{id}/reject
        [HttpPost("appointments/{id}/reject")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            var (succeeded, errorMessage) = await _doctorService.RejectAppointmentAsync(id);
            if (succeeded)
            {
                return Ok(new { Message = "Appointment rejected successfully." });
            }
            return BadRequest(new { Message = errorMessage });
        }

        // POST: api/doctor/appointments/{id}/revisit
        [HttpPost("appointments/{id}/revisit")]
        public async Task<IActionResult> ScheduleRevisit(int id)
        {
            var (createdRevisit, errorMessage) = await _doctorService.ScheduleRevisitAsync(id);
            if (createdRevisit != null)
            {
                return Ok(createdRevisit);
            }
            return BadRequest(new { Message = errorMessage });
        }
    }
}
