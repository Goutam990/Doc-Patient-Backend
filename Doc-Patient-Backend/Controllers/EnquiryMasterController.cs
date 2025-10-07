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
    public class EnquiryMasterController(IEnquiryService enquiryService) : ControllerBase
    {
        // GET: api/EnquiryMaster/statuses
        [HttpGet("statuses")]
        public async Task<IActionResult> GetEnquiryStatuses()
        {
            var list = await enquiryService.GetEnquiryStatusesAsync();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/types
        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var list = await enquiryService.GetAllTypesAsync();
            return Ok(list);
        }

        // GET: api/EnquiryMaster
        [HttpGet]
        public async Task<IActionResult> GetAllEnquiry()
        {
            var list = await enquiryService.GetAllEnquiriesAsync();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnquiryById(int id)
        {
            var enquiry = await enquiryService.GetEnquiryByIdAsync(id);
            if (enquiry == null)
            {
                return NotFound(new { message = "Enquiry not found" });
            }
            return Ok(enquiry);
        }

        // POST: api/EnquiryMaster
        [HttpPost]
        public async Task<IActionResult> AddNewEnquiry([FromBody] EnquiryModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdEnquiry = await enquiryService.AddNewEnquiryAsync(obj);
            return CreatedAtAction(nameof(GetEnquiryById), new { id = createdEnquiry.EnquiryId }, createdEnquiry);
        }

        // PUT: api/EnquiryMaster/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnquiry(int id, [FromBody] EnquiryModel obj)
        {
            var result = await enquiryService.UpdateEnquiryAsync(id, obj);
            if (!result)
            {
                return NotFound(new { message = "Enquiry not found" });
            }
            return Ok(new { message = "Enquiry updated successfully" });
        }

        // DELETE: api/EnquiryMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnquiryById(int id)
        {
            var result = await enquiryService.DeleteEnquiryAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Enquiry not found" });
            }
            return Ok(new { message = "Deleted successfully" });
        }
    }
}