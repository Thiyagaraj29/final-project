

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalManagement.Data;
using RentalManagement.Models;

namespace RentalManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly FlicksDbContext _context;

        public RentalsController(FlicksDbContext context)
        {
            _context = context;
        }

        //  Request Rental (Create with rules)
        [HttpPost]
        public IActionResult RequestRental([FromBody] Rental rental)
        {
            var person = _context.Persons.FirstOrDefault(p => p.UserID == rental.UserID);
            var movie = _context.Movies.FirstOrDefault(m => m.MovieID == rental.MovieID);

            if (person == null || movie == null)
                return BadRequest("Invalid User or Movie.");

            
            int maxLimit = person.MembershipType switch
            {
                "Silver" => 2,
                "Gold" => 3,
                "Platinum" => 5,
                _ => 2
            };

            int currentRentals = _context.Rentals
                .Count(r => r.UserID == rental.UserID && r.Status == "Rented");

            if (currentRentals >= maxLimit)
                return BadRequest($"Limit reached. {person.MembershipType} users can rent only {maxLimit} movies.");

            
            if (movie.Stock <= 0)
                return BadRequest("Movie is out of stock.");

 
            movie.Stock--;

            rental.Status = "Rented";
            rental.RentalDate = DateTime.Now;

            _context.Rentals.Add(rental);
            _context.SaveChanges();

            return Ok(rental);
        }

        //  Get Rentals by User
        [HttpGet("user/{userId}")]
        public IActionResult GetUserRentals(int userId)
        {

            var rentals = _context.Rentals
                .Include(r => r.Movie)
                .Include(r => r.User)   
                .Where(r => r.UserID == userId)
                .ToList();
            var result = rentals.Select(r => new
            {
                r.RentalID,
                r.Status,
                r.RentalDate,
                r.ReturnDate,
                Movie = new
                {
                    r.Movie.MovieID,
                    r.Movie.Title,
                    r.Movie.Stock
                },
                User = new
                {
                    r.User.UserID,
                    r.User.UserName,
                    r.User.MembershipType
                }
            });

            return Ok(result);
        }

        // Approve Rental (Admin)
        [HttpPut("{id}/approve")]
        public IActionResult ApproveRental(int id)
        {
            var rental = _context.Rentals.FirstOrDefault(r => r.RentalID == id);
            if (rental == null || rental.Status != "Booked")
                return BadRequest("Rental not found or cannot be approved");

            rental.Status = "Approved";
            _context.SaveChanges();

            return Ok(rental);
        }

        // Deliver Rental
        [HttpPut("{id}/deliver")]
        public IActionResult DeliverRental(int id)
        {
            var rental = _context.Rentals.FirstOrDefault(r => r.RentalID == id);
            if (rental == null || rental.Status != "Approved")
                return BadRequest("Rental not found or cannot be delivered");

            rental.Status = "Delivered";
            _context.SaveChanges();

            return Ok(rental);
        }

        // Return Rental (increase stock)
        [HttpPut("{id}/return")]
        public IActionResult ReturnRental(int id)
        {
            var rental = _context.Rentals.FirstOrDefault(r => r.RentalID == id);
            if (rental == null || rental.Status != "Delivered")
                return BadRequest("Rental not found or cannot be returned");

            rental.Status = "Returned";
            rental.ReturnDate = DateTime.Now;

            var movie = _context.Movies.FirstOrDefault(m => m.MovieID == rental.MovieID);
            if (movie != null)
                movie.Stock++;

            _context.SaveChanges();

            return Ok(rental);
        }

        // Cancel Rental
        [HttpPut("{id}/cancel")]
        public IActionResult CancelRental(int id)
        {
            var rental = _context.Rentals.FirstOrDefault(r => r.RentalID == id);
            if (rental == null || rental.Status == "Returned")
                return BadRequest("Rental not found or cannot be cancelled");

            rental.Status = "Cancelled";

            
            if (rental.Status == "Rented" || rental.Status == "Approved")
            {
                var movie = _context.Movies.FirstOrDefault(m => m.MovieID == rental.MovieID);
                if (movie != null)
                    movie.Stock++;
            }

            _context.SaveChanges();

            return Ok(rental);
        }

       

       

    }
}

