using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;

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
        public IActionResult GetActiveServiceStatus(string customerId)
        {
            try
            {
                // We fetch only the latest booking that isn't archived
                var latestBooking = _context.Bookings
                    .Where(b => b.CustomerId == customerId)
                    .OrderByDescending(b => b.CreatedAt)
                    .FirstOrDefault();

                if (latestBooking == null)
                {
                    return NotFound(new { message = "No active service records found." });
                }

                return Ok(latestBooking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
    }
}