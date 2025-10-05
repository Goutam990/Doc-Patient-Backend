using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController(ISettingsService settingsService) : ControllerBase
    {
        // GET: api/settings/available-slots?date=YYYY-MM-DD&doctorId=some-guid
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] DateTime date, [FromQuery] string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                return BadRequest("DoctorId is required.");
            }

            var availableSlots = await settingsService.GetAvailableSlotsAsync(date, doctorId);
            return Ok(availableSlots);
        }
    }
}