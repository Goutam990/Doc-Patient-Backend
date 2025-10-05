using Doc_Patient_Backend.Models.DTOs;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // PATCH: api/Appointment/{id}/status
        [HttpPatch("{id}/status")]
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

        // POST: api/Appointment
        [HttpPost]
        public async Task<IActionResult> CreateNewAppointment([FromBody] CreateAppointmentDto obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAppointment = await appointmentService.CreateNewAppointmentAsync(obj);
            return CreatedAtAction(nameof(GetAllAppointments), new { id = createdAppointment.Id }, createdAppointment);
        }
    }
}