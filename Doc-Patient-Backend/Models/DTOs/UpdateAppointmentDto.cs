using System;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class UpdateAppointmentDto
    {
        [Required]
        public DateTime StartTime { get; set; }
    }
}
