using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize(Roles = UserRoles.Doctor)]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController(IDoctorAvailabilityService availabilityService) : ControllerBase
    {
        // GET: api/doctoravailability
        [HttpGet]
        public async Task<IActionResult> GetMyAvailability()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vacationDays = await availabilityService.GetVacationDaysAsync(doctorId);
            return Ok(vacationDays);
        }

        // POST: api/doctoravailability
        [HttpPost]
        public async Task<IActionResult> AddVacationDay([FromBody] DoctorAvailability request)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure a doctor can only add availability for themselves
            if (request.DoctorId != doctorId)
            {
                return Forbid();
            }

            var newAvailability = await availabilityService.AddVacationDayAsync(doctorId, request.VacationDate, request.Reason);
            return CreatedAtAction(nameof(GetMyAvailability), new { id = newAvailability.Id }, newAvailability);
        }

        // DELETE: api/doctoravailability/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacationDay(int id)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await availabilityService.DeleteVacationDayAsync(id, doctorId);

            if (!result)
            {
                return NotFound(new { message = "Availability record not found or you do not have permission to delete it." });
            }

            return NoContent();
        }
    }
}