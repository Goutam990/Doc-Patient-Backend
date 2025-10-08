using System;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class CreateAppointmentDto
    {
        public int? Age { get; set; }
        public string? Gender { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        [Required]
        public string DoctorId { get; set; }
        [Required]
        public string PaymentIntentId { get; set; }
    }
}