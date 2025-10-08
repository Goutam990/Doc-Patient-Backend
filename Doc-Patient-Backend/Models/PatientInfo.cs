using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    public class PatientInfo
    {
        [Key]
        public int Id { get; set; }

        public string? IllnessHistory { get; set; }

        // Foreign key to link to the user
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}