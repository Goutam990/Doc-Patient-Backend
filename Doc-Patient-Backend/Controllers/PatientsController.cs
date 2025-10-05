using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Doctor}")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController(IPatientsService patientsService) : ControllerBase
    {
        // GET: api/patients
        [HttpGet]
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

        // PATCH: api/patients/{id}/block
        [HttpPatch("{id}/block")]
        public async Task<IActionResult> ToggleBlockPatient(string id)
        {
            var user = await patientsService.ToggleBlockPatientAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Patient not found." });
            }

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.PhoneNumber,
                user.IsBlocked
            });
        }
    }
}