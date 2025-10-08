using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
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