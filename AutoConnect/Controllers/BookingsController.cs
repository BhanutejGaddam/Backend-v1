using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Net.Http.Json;

namespace UserAuthApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            if (booking == null) return BadRequest("Invalid data");

            try
            {
                var allDealers = await _context.Dealers.ToListAsync();

                // If there are no dealers in the system at all, we can't assign anyone
                if (allDealers == null || !allDealers.Any())
                    return BadRequest("No dealers available in the system.");

                AddDealer? closestDealer = null;
                double minDistance = double.MaxValue;

                string apiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6ImM1MTI3N2IzZDcyZTQyMDJhMmFlMzJjN2UxMWMzNTUwIiwiaCI6Im11cm11cjY0In0=";

                foreach (var dealer in allDealers)
                {
                    double distance = await GetORSDistanceAsync(booking.Address, dealer.StoreAddress, apiKey);

                    // If we get a valid distance and it's shorter than what we've seen...
                    if (distance < double.MaxValue && distance < minDistance)
                    {
                        minDistance = distance;
                        closestDealer = dealer;
                    }
                }

                // --- FALLBACK LOGIC ---
                if (closestDealer != null)
                {
                    // Successfully found via Map API
                    booking.Selected_Dealer = closestDealer.StoreName;
                    booking.Selected_Dealer_Id = closestDealer.DealerId;
                }
                else
                {
                    // Map API failed or address was not found. 
                    // Assign the FIRST dealer in the database as the Default.
                    var defaultDealer = allDealers.First();
                    booking.Selected_Dealer = defaultDealer.StoreName;
                    booking.Selected_Dealer_Id = defaultDealer.DealerId;

                    // Optional: Tag the booking so you know it was a fallback
                    // booking.DescriptionOfIssues += " (Assigned to Default Dealer)";
                }

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Booking Successful", dealer = booking.Selected_Dealer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        

        private async Task<double> GetORSDistanceAsync(string startAddr, string endAddr, string key)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "VehicleServiceApp");

                // Encode addresses to handle spaces/special characters
                var url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={key}&start={Uri.EscapeDataString(startAddr)}&end={Uri.EscapeDataString(endAddr)}";

                var response = await client.GetFromJsonAsync<ORSResponse>(url);

                if (response?.Features != null && response.Features.Count > 0)
                {
                    // Return distance in meters
                    return response.Features[0].Properties.Segments[0].Distance;
                }
            }
            catch
            {
                // Log error if necessary
            }
            return double.MaxValue;
        }
    }

    // Helper classes for JSON Deserialization
    public class ORSResponse { public List<ORSFeature> Features { get; set; } = new(); }
    public class ORSFeature { public ORSProperty Properties { get; set; } = new(); }
    public class ORSProperty { public List<ORSSegment> Segments { get; set; } = new(); }
    public class ORSSegment { public double Distance { get; set; } }
}