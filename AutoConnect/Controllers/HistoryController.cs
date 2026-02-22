using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for async EF Core extensions
using UserAuthApi.Data;
using UserAuthApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks; // Required for Task

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
        public async Task<IActionResult> GetCompletedHistory(string customerId) // Changed to async Task
        {
            try
            {
                // Filter by CustomerId AND status must be 'COMPLETED'
                // Replaced ToList() with ToListAsync() and added await
                var history = await _context.Bookings
                    .Where(b => b.CustomerId == customerId && b.BookingStatus == "COMPLETED")
                    .OrderByDescending(b => b.Slot)
                    .ToListAsync();

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching history", error = ex.Message });
            }
        }
    }
}