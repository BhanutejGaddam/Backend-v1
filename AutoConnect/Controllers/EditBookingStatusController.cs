using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "dealer")]
    public class EditBookingStatusController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EditBookingStatusController(AppDbContext context)
        {
            _context = context;
        }

        // PUT: api/BookingStatus/update-status/101
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] StatusUpdateDto dto)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);

                if (booking == null)
                    return NotFound(new { message = "Booking not found in database." });

                // Update the status column
                booking.BookingStatus = dto.NewStatus;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error during update", error = ex.Message });
            }
        }
    }

    public class StatusUpdateDto
    {
        public string NewStatus { get; set; } = string.Empty;
    }
}