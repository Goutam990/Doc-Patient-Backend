using Doc_Patient_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly ApplicationDbContext _context;

        public EnquiryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnquiryStatus>> GetEnquiryStatusesAsync()
        {
            return await _context.EnquiryStatuses.ToListAsync();
        }

        public async Task<IEnumerable<EnquiryType>> GetAllTypesAsync()
        {
            return await _context.EnquiryTypes.ToListAsync();
        }

        public async Task<IEnumerable<EnquiryModel>> GetAllEnquiriesAsync()
        {
            return await _context.EnquiryModels.ToListAsync();
        }

        public async Task<EnquiryModel> GetEnquiryByIdAsync(int id)
        {
            return await _context.EnquiryModels.FindAsync(id);
        }

        public async Task<EnquiryModel> AddNewEnquiryAsync(EnquiryModel enquiry)
        {
            enquiry.createdAt = DateTime.Now;
            _context.EnquiryModels.Add(enquiry);
            await _context.SaveChangesAsync();
            return enquiry;
        }

        public async Task<bool> UpdateEnquiryAsync(int id, EnquiryModel enquiry)
        {
            var record = await _context.EnquiryModels.SingleOrDefaultAsync(m => m.enquiryId == id);
            if (record == null)
            {
                return false;
            }

            record.resolution = enquiry.resolution;
            record.enquiryStatusId = enquiry.enquiryStatusId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEnquiryAsync(int id)
        {
            var record = await _context.EnquiryModels.SingleOrDefaultAsync(m => m.enquiryId == id);
            if (record == null)
            {
                return false;
            }

            _context.EnquiryModels.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}