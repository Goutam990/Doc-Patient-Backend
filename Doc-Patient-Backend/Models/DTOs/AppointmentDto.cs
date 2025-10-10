using System;

namespace Doc_Patient_Backend.Models.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public bool IsRevisit { get; set; }
        // We can add patient/doctor details here later if needed
    }
}
