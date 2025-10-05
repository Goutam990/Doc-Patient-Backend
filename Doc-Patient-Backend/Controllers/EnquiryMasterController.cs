using Doc_Patient_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Doc_Patient_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryMasterController(ApplicationDbContext context) : ControllerBase
    {

        // GET: api/EnquiryMaster/statuses
        [HttpGet("statuses")]
        public ActionResult<List<EnquiryStatus>> GetEnquiryStatuses()
        {
            var list = context.EnquiryStatuses.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/types
        [HttpGet("types")]
        public ActionResult<List<EnquiryType>> GetAllTypes()
        {
            var list = context.EnquiryTypes.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster
        [HttpGet]
        public ActionResult<List<EnquiryModel>> GetAllEnquiry()
        {
            var list = context.EnquiryModels.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/5
        [HttpGet("{id}")]
        public ActionResult<EnquiryModel> GetEnquiryById(int id)
        {
            var enquiry = context.EnquiryModels.Find(id);

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
            context.EnquiryModels.Add(obj);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetEnquiryById), new { id = obj.enquiryId }, obj);
        }

        // PUT: api/EnquiryMaster/5
        [HttpPut("{id}")]
        public IActionResult UpdateEnquiry(int id, [FromBody] EnquiryModel obj)
        {
            var record = context.EnquiryModels.SingleOrDefault(m => m.enquiryId == id);
            if (record == null)
            {
                return NotFound(new { message = "Enquiry not found" });
            }

            record.resolution = obj.resolution;
            record.enquiryStatusId = obj.enquiryStatusId;
            context.SaveChanges();

            return Ok(new { message = "Enquiry updated successfully" });
        }

        // DELETE: api/EnquiryMaster/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEnquiryById(int id)
        {
            var record = context.EnquiryModels.SingleOrDefault(m => m.enquiryId == id);
            if (record == null)
            {
                return NotFound(new { message = "Enquiry not found" });
            }

            context.EnquiryModels.Remove(record);
            context.SaveChanges();
            return Ok(new { message = "Deleted successfully" });
        }
    }
}