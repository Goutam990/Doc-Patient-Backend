using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    [Table("EnquiryType")]
    public class EnquiryType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int typeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string typeName { get; set; } = string.Empty;
    }
}
