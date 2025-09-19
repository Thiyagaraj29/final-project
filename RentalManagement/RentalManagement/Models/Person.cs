using System.ComponentModel.DataAnnotations;

namespace RentalManagement.Models
{
    public class Person
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(20)]
        public string MembershipType { get; set; }
    }
}
