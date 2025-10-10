using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    public enum ActionType
    {
        Created,
        Edited,
        Cancelled,
        AutoCancelled,
        Rescheduled,
        Approved, // Added for clarity
        Rejected  // Added for clarity
    }

    public enum PerformedBy
    {
        Doctor,
        Patient,
        System
    }

    public class AppointmentLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }

        [Required]
        public ActionType ActionType { get; set; }

        [Required]
        public PerformedBy PerformedBy { get; set; }

        public DateTime? OldTime { get; set; }
        public DateTime? NewTime { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}