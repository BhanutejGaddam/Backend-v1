using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")] // This makes the URL: api/Bookings
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost] // The URL will be: api/Bookings
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            if (booking == null) return BadRequest("Invalid data");

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return Ok(new { message = "Booking successful" });
        }
    }
}