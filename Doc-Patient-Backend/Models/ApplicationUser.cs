using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsBlocked { get; set; } = false;

        public string? Specialization { get; set; } // For Doctors
        public int? Experience { get; set; } // For Doctors
        public string? Gender { get; set; } // For Patients
        public DateTime? DOB { get; set; } // For Patients
    }
}