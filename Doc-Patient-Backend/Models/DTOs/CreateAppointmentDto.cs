using System;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public string PatientName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public string AppointmentTime { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        [Required]
        public string PatientId { get; set; }
        [Required]
        public string DoctorId { get; set; }
    }
}