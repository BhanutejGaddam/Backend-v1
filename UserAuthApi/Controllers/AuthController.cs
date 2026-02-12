using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CustomerInfo customer)
        {
            if (customer == null)
                return BadRequest(new { message = "Invalid customer data." });

            // 1. Business Logic: Default to 'self' if no dealer is provided
            if (string.IsNullOrWhiteSpace(customer.AddedByDealer))
            {
                customer.AddedByDealer = "self";
            }

            // 2. Pre-check: Prevent duplicate Emails (Assuming Email is unique in your table)
            if (_context.Customers.Any(c => c.CMailId == customer.CMailId))
            {
                return Conflict(new { message = "A user with this email already exists." });
            }

            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return Ok(new { message = "Registration Successful" });
            }
            catch (DbUpdateException ex)
            {
                // Handle database-level constraints (Foreign Keys, Unique Indexes)
                var innerMessage = ex.InnerException?.Message ?? ex.Message;

                // Log the full exception here for debugging
                return BadRequest(new
                {
                    error = "Database Constraint Violation",
                    details = innerMessage.Contains("FOREIGN KEY")
                              ? "The associated Dealer or reference was not found."
                              : "Could not save record due to database constraints."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal Server Error", details = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email))
                return BadRequest(new { success = false, message = "Invalid request" });

            // Use a unified way to store the found ID and Role
            string? foundId = null;
            string? foundRole = null;

            // 1. Check Customer Table
            if (request.Role == "customer")
            {
                var user = _context.Customers.FirstOrDefault(c => c.CMailId == request.Email && c.CPassword == request.Password);
                if (user != null)
                {
                    foundId = user.CustomerId;
                    foundRole = "customer";
                }
            }
            // 2. Check Dealer Table
            else if (request.Role == "dealer")
            {
                var dealer = _context.Dealers.FirstOrDefault(d => d.DMailId == request.Email && d.DPassword == request.Password);
                if (dealer != null)
                {
                    foundId = dealer.DealerId;
                    foundRole = "dealer";
                }
            }
            // 3. Check Admin Table
            else if (request.Role == "admin")
            {
                var admin = _context.Admins.FirstOrDefault(a => a.AMailId == request.Email && a.APassword == request.Password);
                if (admin != null)
                {
                    foundId = admin.AdminId;
                    foundRole = "admin";
                }
            }

            // 4. Final Response Logic
            if (foundId != null)
            {
                return Ok(new
                {
                    success = true,
                    customerId = foundId, // Angular looks for this key to save to localStorage
                    role = foundRole
                });
            }

            // If we reached here, no match was found in any table
            return Unauthorized(new { success = false, message = "Invalid credentials" });
        }


        [HttpPost("book-service")]
        public IActionResult BookService([FromBody] Booking booking)
        {
            if (booking == null) return BadRequest();
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return Ok(new { message = "Booking successful" });
        }

        [HttpGet("track-service/{customerId}")]
        public IActionResult GetLatestBooking(string customerId)
        {
            // Fetch the most recent booking for this specific customer
            var booking = _context.Bookings
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefault();

            if (booking == null)
            {
                return NotFound(new { message = "No booking found for this customer." });
            }

            return Ok(booking);
        }

        [HttpGet("service-history/{customerId}")]
        public IActionResult GetServiceHistory(string customerId)
        {
            var history = _context.Bookings
                .Where(b => b.CustomerId == customerId && b.BookingStatus == "COMPLETED")
                .OrderByDescending(b => b.Slot)
                .ToList();

            return Ok(history);
        }
    }
}