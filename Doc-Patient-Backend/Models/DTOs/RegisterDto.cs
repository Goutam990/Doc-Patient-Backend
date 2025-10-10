using System;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class RegisterDto
    {
        // User Info
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // Patient Info
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [StringLength(5)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        public string IllnessHistory { get; set; }

        public string Picture { get; set; } // URL
    }
}
