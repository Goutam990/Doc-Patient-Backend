using Doc_Patient_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doc_Patient_Backend.Services
{
    public interface IEnquiryService
    {
        Task<IEnumerable<EnquiryStatus>> GetEnquiryStatusesAsync();
        Task<IEnumerable<EnquiryType>> GetAllTypesAsync();
        Task<IEnumerable<EnquiryModel>> GetAllEnquiriesAsync();
        Task<EnquiryModel> GetEnquiryByIdAsync(int id);
        Task<EnquiryModel> AddNewEnquiryAsync(EnquiryModel enquiry);
        Task<bool> UpdateEnquiryAsync(int id, EnquiryModel enquiry);
        Task<bool> DeleteEnquiryAsync(int id);
    }
}