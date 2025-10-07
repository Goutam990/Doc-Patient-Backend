using System;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
    }
}