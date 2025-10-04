using System;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class NewAppointment
    {
        [Required]
        public string PatientName { get; set; }

        public string Email { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
    }
}