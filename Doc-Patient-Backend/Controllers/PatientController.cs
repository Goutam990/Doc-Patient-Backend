using Doc_Patient_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Patient
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            var patients = _context.Patients.ToList();
            return Ok(patients);
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public IActionResult GetPatientById(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }

            return Ok(patient);
        }

        // GET: api/Patient/mobile/1234567890
        [HttpGet("mobile/{mobile}")]
        public IActionResult GetPatientByMobileNo(string mobile)
        {
            var patient = _context.Patients.SingleOrDefault(p => p.MobileNo == mobile);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }

            return Ok(patient);
        }

        // POST: api/Patient
        [HttpPost]
        public IActionResult CreatePatient([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
        }

        // PUT: api/Patient/5
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }

            patient.PatientName = updatedPatient.PatientName;
            patient.Email = updatedPatient.Email;
            patient.MobileNo = updatedPatient.MobileNo;
            patient.City = updatedPatient.City;
            patient.Address = updatedPatient.Address;

            _context.SaveChanges();

            return Ok(new { message = "Patient updated successfully" });
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();

            return Ok(new { message = "Patient deleted successfully" });
        }
    }
}