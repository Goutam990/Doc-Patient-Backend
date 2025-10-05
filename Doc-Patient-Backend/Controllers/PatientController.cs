using Doc_Patient_Backend.Models;
using Doc_Patient_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController(IPatientService patientService) : ControllerBase
    {
        // GET: api/Patient
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }
            return Ok(patient);
        }

        // GET: api/Patient/mobile/1234567890
        [HttpGet("mobile/{mobile}")]
        public async Task<IActionResult> GetPatientByMobileNo(string mobile)
        {
            var patient = await patientService.GetPatientByMobileNoAsync(mobile);
            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }
            return Ok(patient);
        }

        // POST: api/Patient
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdPatient = await patientService.CreatePatientAsync(patient);
            return CreatedAtAction(nameof(GetPatientById), new { id = createdPatient.PatientId }, createdPatient);
        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            var result = await patientService.UpdatePatientAsync(id, updatedPatient);
            if (!result)
            {
                return NotFound(new { message = "Patient not found" });
            }
            return Ok(new { message = "Patient updated successfully" });
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await patientService.DeletePatientAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Patient not found" });
            }
            return Ok(new { message = "Patient deleted successfully" });
        }
    }
}