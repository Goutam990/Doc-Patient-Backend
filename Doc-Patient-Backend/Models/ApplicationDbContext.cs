using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EnquiryModel> EnquiryModels { get; set; }
        public DbSet<EnquiryStatus> EnquiryStatuses { get; set; }
        public DbSet<EnquiryType> EnquiryTypes { get; set; }
        public DbSet<PatientInfo> PatientInfos { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Appointment relationships
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(u => u.AppointmentsAsPatient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(u => u.AppointmentsAsDoctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // PatientInfo one-to-one relationship with ApplicationUser
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.PatientInfo)
                .WithOne(pi => pi.User)
                .HasForeignKey<PatientInfo>(pi => pi.UserId);

            // DoctorAvailability one-to-many relationship with ApplicationUser
            builder.Entity<DoctorAvailability>()
                .HasOne(da => da.Doctor)
                .WithMany()
                .HasForeignKey(da => da.DoctorId);
        }
    }

    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PatientName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; } = "Pending"; // New status flow: Pending -> Confirmed
        public string? PaymentIntentId { get; set; }

        // Foreign key to link to the user (patient)
        public string PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual ApplicationUser Patient { get; set; }

        // Foreign key to link to the user (doctor)
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual ApplicationUser Doctor { get; set; }
    }
}