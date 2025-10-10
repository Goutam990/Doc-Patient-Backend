using System;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class UpdateAvailabilityDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
