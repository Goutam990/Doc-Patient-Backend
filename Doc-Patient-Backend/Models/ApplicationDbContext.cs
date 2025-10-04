using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EnquiryModel> EnquiryModels { get; set; }
        public DbSet<EnquiryStatus> EnquiryStatuses { get; set; }
        public DbSet<EnquiryType> EnquiryTypes { get; set; }
    }

    [Table("Patients")]
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        public string PatientName { get; set; }

        public string Email { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }
    }

    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public bool IsDone { get; set; }

        public double? Fees { get; set; }
    }
}