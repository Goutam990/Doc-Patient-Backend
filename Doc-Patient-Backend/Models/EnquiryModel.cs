using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doc_Patient_Backend.Models
{
    [Table("Enquiry")]
    public class EnquiryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int enquiryId { get; set; }

        [Required]
        public int enquiryTypeId { get; set; }

        [Required]
        public int enquiryStatusId { get; set; }

        [Required]
        [MaxLength(100)]
        public string customerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string mobileNo { get; set; } = string.Empty;

        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [MaxLength(500)]
        public string message { get; set; } = string.Empty;

        [MaxLength(500)]
        public string resolution { get; set; } = string.Empty;

        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
