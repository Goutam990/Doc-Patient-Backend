using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    // DbContext for Appointments
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }

    // Patient entity
    [Table("patient")]
    public class Patient
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        public string MobileNo { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }

    // DTO for creating a new appointment
    public class NewAppointment
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required]
        public string MobileNo { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime AppointmentDate { get; set; }
    }

    // Appointment entity
    [Table("Appointment")]
    public class Appointment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public bool IsDone { get; set; }

        public double? Fees { get; set; }
    }
}
