using Microsoft.EntityFrameworkCore;

namespace Doc_Patient_Backend.Models
{
    public class EnquiryDbContext : DbContext
    {
        public EnquiryDbContext(DbContextOptions<EnquiryDbContext> options) : base(options)
        {
        }

        // ✅ CORRECT THE TYPO: EnquirieStatuses -> EnquiryStatuses
        public DbSet<EnquiryStatus> EnquiryStatuses { get; set; }

        public DbSet<EnquiryType> EnquiryTypes { get; set; }

        public DbSet<EnquiryModel> EnquiryModels { get; set; }
    }
}