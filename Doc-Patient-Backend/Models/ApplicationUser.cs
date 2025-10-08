using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Doc_Patient_Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            AppointmentsAsPatient = new HashSet<Appointment>();
            AppointmentsAsDoctor = new HashSet<Appointment>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool IsBlocked { get; set; } = false;

        public string? Specialization { get; set; } // For Doctors
        public int? Experience { get; set; } // For Doctors
        public string? Gender { get; set; } // For Patients
        public DateTime? DOB { get; set; } // For Patients

        public virtual ICollection<Appointment> AppointmentsAsPatient { get; set; }
        public virtual ICollection<Appointment> AppointmentsAsDoctor { get; set; }
        public virtual PatientInfo PatientInfo { get; set; }
    }
}