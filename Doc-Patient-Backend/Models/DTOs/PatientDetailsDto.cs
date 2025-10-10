using System;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class PatientDetailsDto
    {
        public int Id { get; set; } // PatientInfo Id
        public string UserId { get; set; } // AspNetUsers Id
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string IllnessHistory { get; set; }
        public string Picture { get; set; }
    }
}
