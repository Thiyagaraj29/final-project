using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManagement.Models
{
    //public class Rental
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-increment PK
    //    public int RentalID { get; set; }

    //    [Required]
    //    [ForeignKey("User")]   // FK -> Users table
    //    public int UserID { get; set; }

    //    [Required]
    //    [ForeignKey("Movie")]  // FK -> Movies table
    //    public int MovieID { get; set; }

    //    [Required]
    //    [StringLength(20)]
    //    public string Status { get; set; }   // Rented, Returned, Cancelled

    //    [Required]
    //    public DateTime RentalDate { get; set; } = DateTime.Now;

    //    public DateTime? ReturnDate { get; set; }
    //}
    public class Rental
    {
        [Key]
        public int RentalID { get; set; }

        [Required]
        public int UserID { get; set; } // FK to Person

        [ForeignKey("UserID")]
        public Person User { get; set; } // Navigation uses the correct FK

        [Required]
        public int MovieID { get; set; } // FK to Movie

        [ForeignKey("MovieID")]
        public Movie Movie { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        public DateTime RentalDate { get; set; } = DateTime.Now;

        public DateTime? ReturnDate { get; set; }
    }

}
