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

        // PUT: api/EditBookingStatus/update-status/101
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] StatusUpdateDto dto)
        {
            // FindAsync will return null if the ID doesn't exist
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound(new { message = "Booking not found in database." });

            // Update the status column
            booking.BookingStatus = dto.NewStatus;

            // Save changes - Middleware catches potential DB errors
            await _context.SaveChangesAsync();

            return Ok(new { message = "Status updated successfully" });
        }
    }

    public class StatusUpdateDto
    {
        public string NewStatus { get; set; } = string.Empty;
    }
}