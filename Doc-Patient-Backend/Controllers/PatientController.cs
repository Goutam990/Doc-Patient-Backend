using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize(Roles = UserRoles.Patient)]
    [Route("api/patient")]
    [ApiController]
    public class PatientController(UserManager<ApplicationUser> userManager, IAppointmentService appointmentService) : ControllerBase
    {
        [HttpGet("appointments/upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var patientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(patientId))
            {
                return Unauthorized();
            }

            var appointments = await appointmentService.GetUpcomingAppointmentsForPatientAsync(patientId);
            return Ok(appointments);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetPatientProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

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
                user.DOB,
                user.Gender
            });
        }
    }
}