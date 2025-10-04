using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    [Table("EnquiryStatus")]
    public class EnquiryStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int statusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string status { get; set; } = string.Empty;
    }
}
