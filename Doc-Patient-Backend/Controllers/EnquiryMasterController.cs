using Doc_Patient_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Doc_Patient_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryMasterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnquiryMasterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EnquiryMaster/statuses
        [HttpGet("statuses")]
        public ActionResult<List<EnquiryStatus>> GetEnquiryStatuses()
        {
            var list = _context.EnquiryStatuses.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/types
        [HttpGet("types")]
        public ActionResult<List<EnquiryType>> GetAllTypes()
        {
            var list = _context.EnquiryTypes.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster
        [HttpGet]
        public ActionResult<List<EnquiryModel>> GetAllEnquiry()
        {
            var list = _context.EnquiryModels.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/5
        [HttpGet("{id}")]
        public ActionResult<EnquiryModel> GetEnquiryById(int id)
        {
            var enquiry = _context.EnquiryModels.Find(id);

            if (enquiry == null)
            {
                return NotFound(new { message = "Enquiry not found" });
            }

            return Ok(enquiry);
        }

        // POST: api/EnquiryMaster
        [HttpPost]
        public ActionResult<EnquiryModel> AddNewEnquiry([FromBody] EnquiryModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            obj.createdAt = DateTime.Now;
            _context.EnquiryModels.Add(obj);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetEnquiryById), new { id = obj.enquiryId }, obj);
        }

        // PUT: api/EnquiryMaster/5
        [HttpPut("{id}")]
        public IActionResult UpdateEnquiry(int id, [FromBody] EnquiryModel obj)
        {
            var record = _context.EnquiryModels.SingleOrDefault(m => m.enquiryId == id);
            if (record == null)
            {
                return NotFound(new { message = "Enquiry not found" });
            }

            record.resolution = obj.resolution;
            record.enquiryStatusId = obj.enquiryStatusId;
            _context.SaveChanges();

            return Ok(new { message = "Enquiry updated successfully" });
        }

        // DELETE: api/EnquiryMaster/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEnquiryById(int id)
        {
            var record = _context.EnquiryModels.SingleOrDefault(m => m.enquiryId == id);
            if (record == null)
            {
                return NotFound(new { message = "Enquiry not found" });
            }

            _context.EnquiryModels.Remove(record);
            _context.SaveChanges();
            return Ok(new { message = "Deleted successfully" });
        }
    }
}