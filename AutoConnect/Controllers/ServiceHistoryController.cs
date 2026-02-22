using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "dealer")] // Ensure only dealers can view this
    public class ServiceHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceHistoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceHistory/customer/CUS0001
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetServiceHistory(string customerId)
        {
            try
            {
                // Fetch all bookings for this specific customer
                var history = await _context.Bookings
                    .Where(b => b.CustomerId == customerId)
                    .OrderByDescending(b => b.Slot) // Newest appointments first
                    .ToListAsync();

                if (history == null || !history.Any())
                {
                    return NotFound(new { message = "No service history found for this customer." });
                }

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving service history", error = ex.Message });
            }
        }
    }
}