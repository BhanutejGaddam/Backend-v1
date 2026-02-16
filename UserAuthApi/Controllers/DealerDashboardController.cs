using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Security.Claims;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "dealer")]
    public class DealerDashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DealerDashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("today-bookings")]
        public async Task<IActionResult> GetTodayBookings()
        {
            try
            {
                // 1. Get the Dealer ID from the authenticated token
                var currentDealerId = User.FindFirst("db_id")?.Value;

                if (string.IsNullOrEmpty(currentDealerId))
                    return Unauthorized(new { message = "Dealer ID not found in token." });

                // 2. Get today's date (at midnight)
                var today = DateTime.Today;

                // 3. Query bookings for this dealer where the Slot is today
                var bookings = await _context.Bookings
                    .Where(b => b.Selected_Dealer_Id == currentDealerId &&
                                b.Slot == DateTime.Today)
                    .OrderBy(b => b.Slot) // Order by appointment time
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching today's bookings", error = ex.Message });
            }
        }
    }
}