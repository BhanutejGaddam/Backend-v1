using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetCompletedHistory(string customerId)
        {
            // Filter by CustomerId AND status must be 'COMPLETED'
            // The Middleware catches any database-related exceptions here
            var history = await _context.Bookings
                .Where(b => b.CustomerId == customerId && b.BookingStatus == "COMPLETED")
                .OrderByDescending(b => b.Slot)
                .ToListAsync();

            return Ok(history);
        }
    }
}