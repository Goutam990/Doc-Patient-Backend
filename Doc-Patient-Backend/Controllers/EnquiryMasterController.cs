using Doc_Patient_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace Doc_Patient_Backend.Controllers
{
    [Route("api/[controller]")] // ❌ Remove spaces
    [ApiController]
    public class EnquiryMasterController : ControllerBase // Use ControllerBase for API
    {
        private readonly EnquiryDbContext _context;

        public EnquiryMasterController(EnquiryDbContext context)
        {
            _context = context;
        }

        // GET: api/EnquiryMaster/GetAllStatus
        [HttpGet("GetAllStatus")]
        public ActionResult<List<EnquiryStatus>> GetEnquiryStatuses()
        {
            var list = _context.EnquiryStatuses.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/GetAllTypes
        [HttpGet("GetAllTypes")]
        public ActionResult<List<EnquiryType>> GetAllTypes()
        {
            var list = _context.EnquiryTypes.ToList();
            return Ok(list);
        }

        // GET: api/EnquiryMaster/GetAllEnquiry
        [HttpGet("GetAllEnquiry")]
        public ActionResult<List<EnquiryModel>> GetAllEnquiry()
        {
            var list = _context.EnquiryModels.ToList();
            return Ok(list);
        }

        // POST: api/EnquiryMaster/CreateNewEnquiry
        [HttpPost("CreateNewEnquiry")]
        public ActionResult<EnquiryModel> AddNewEnquiry(EnquiryModel obj)
        {
            obj.createdAt = DateTime.Now;
            _context.EnquiryModels.Add(obj);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAllEnquiry), new { id = obj.enquiryId }, obj);
        }

        // PUT: api/EnquiryMaster/UpdateEnquiry
        [HttpPut("UpdateEnquiry")]
        public ActionResult<EnquiryModel> UpdateEnquiry(EnquiryModel obj)
        {
            var record = _context.EnquiryModels.SingleOrDefault(m => m.enquiryId == obj.enquiryId);
            if (record == null)
                return NotFound("Enquiry not found");

            record.resolution = obj.resolution;
            record.enquiryStatusId = obj.enquiryStatusId;
            _context.SaveChanges();

            return Ok(record);
        }

        // DELETE: api/EnquiryMaster/DeleteEnquiryById/5
        [HttpDelete("DeleteEnquiryById/{id}")]
        public ActionResult DeleteEnquiryById(int id)
        {
            var record = _context.EnquiryModels.SingleOrDefault(m => m.enquiryId == id);
            if (record == null)
                return NotFound("Enquiry not found");

            _context.EnquiryModels.Remove(record);
            _context.SaveChanges();
            return Ok("Deleted successfully");
        }
    }
}
