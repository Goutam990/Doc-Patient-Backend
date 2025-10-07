using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/admin")]
    [ApiController]
    public class AdminController(IPatientsService patientsService, IAppointmentService appointmentService) : ControllerBase
    {
        // GET: api/admin/patients
        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (patients, totalCount) = await patientsService.GetPatientsAsync(page, pageSize);

            return Ok(new
            {
                Patients = patients.Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Email,
                    p.PhoneNumber,
                    p.IsBlocked
                }),
                TotalCount = totalCount
            });
        }

        // GET: api/admin/appointments
        [HttpGet("appointments")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }
    }
}