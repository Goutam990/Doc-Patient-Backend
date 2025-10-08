using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    public class DoctorAvailability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual ApplicationUser Doctor { get; set; }

        [Required]
        public DateTime VacationDate { get; set; }

        public string? Reason { get; set; } // Optional reason for unavailability
    }
}