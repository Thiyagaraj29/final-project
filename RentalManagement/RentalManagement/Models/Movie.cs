using System.ComponentModel.DataAnnotations;

namespace RentalManagement.Models
{
    public class Movie
    {
            [Key]
            public int MovieID { get; set; }

            [Required]
            [MaxLength(200)]
            public string Title { get; set; }

            [Required]
            public int Stock { get; set; }  // available copies
    
    }
}
