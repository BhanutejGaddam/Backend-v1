using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;
using Microsoft.EntityFrameworkCore;

namespace UserAuthApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrackingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tracking/status/CUST123
        [HttpGet("status/{customerId}")]
        public async Task<IActionResult> GetActiveServiceStatus(string customerId)
        {
            try
            {
                // Fetch all bookings for this customer that are NOT 'COMPLETED'
                var activeBookings = await _context.Bookings
                    .Where(b => b.CustomerId == customerId && b.BookingStatus != "COMPLETED")
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                if (activeBookings == null)
                {
                    return NotFound(new { message = "No active service records found." });
                }

                return Ok(activeBookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
    }
}