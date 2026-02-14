using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/History/customer/CUST123
        [HttpGet("customer/{customerId}")]
        public IActionResult GetCompletedHistory(string customerId)
        {
            try
            {
                // Filter by CustomerId AND status must be 'COMPLETED'
                var history = _context.Bookings
                    .Where(b => b.CustomerId == customerId && b.BookingStatus == "COMPLETED")
                    .OrderByDescending(b => b.Slot)
                    .ToList();

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching history", error = ex.Message });
            }
        }
    }
}