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
        // GET: api/Appointment/GetAllAppointment
        [HttpGet("GetAllAppointment")]
        public async Task<IActionResult> GetAllAppointment()
        {
            var list = await appointmentService.GetAllAppointmentsAsync();
            return Ok(list);
        }

        // GET: api/Appointment/GetDoneAppointment
        [HttpGet("GetDoneAppointment")]
        public async Task<IActionResult> GetDoneAppointment()
        {
            var list = await appointmentService.GetDoneAppointmentsAsync();
            return Ok(list);
        }

        // PUT: api/Appointment/{appointmentId}/status
        [HttpPut("{appointmentId}/status")]
        public async Task<IActionResult> ChangeStatus(int appointmentId, [FromBody] bool isDone)
        {
            var result = await appointmentService.ChangeAppointmentStatusAsync(appointmentId, isDone);
            if (!result)
            {
                return NotFound(new { message = "Appointment not found" });
            }
            return Ok(new { message = "Status Changed Successfully" });
        }

        // POST: api/Appointment/CreateNewAppointment
        [HttpPost("CreateNewAppointment")]
        public async Task<IActionResult> CreateNewAppointment([FromBody] NewAppointment obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdAppointment = await appointmentService.CreateNewAppointmentAsync(obj);
            return Created("Appointment Created", createdAppointment);
        }
    }
}