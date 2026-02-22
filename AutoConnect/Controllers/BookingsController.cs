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

            // Global Middleware will now catch any DB or logic exceptions below
            var allDealers = await _context.Dealers.ToListAsync();

            if (allDealers == null || !allDealers.Any())
                return BadRequest("No dealers available in the system.");

            AddDealer? closestDealer = null;
            double minDistance = double.MaxValue;

            string apiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6ImM1MTI3N2IzZDcyZTQyMDJhMmFlMzJjN2UxMWMzNTUwIiwiaCI6Im11cm11cjY0In0=";

            foreach (var dealer in allDealers)
            {
                double distance = await GetORSDistanceAsync(booking.Address, dealer.StoreAddress, apiKey);

                if (distance < double.MaxValue && distance < minDistance)
                {
                    minDistance = distance;
                    closestDealer = dealer;
                }
            }

            // --- FALLBACK LOGIC ---
            if (closestDealer != null)
            {
                booking.Selected_Dealer = closestDealer.StoreName;
                booking.Selected_Dealer_Id = closestDealer.DealerId;
            }
            else
            {
                var defaultDealer = allDealers.First();
                booking.Selected_Dealer = defaultDealer.StoreName;
                booking.Selected_Dealer_Id = defaultDealer.DealerId;
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking Successful", dealer = booking.Selected_Dealer });
        }

        private async Task<double> GetORSDistanceAsync(string startAddr, string endAddr, string key)
        {
            // We keep this local try-catch because an API failure here 
            // shouldn't crash the whole booking; it should just trigger the fallback.
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "VehicleServiceApp");

                var url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={key}&start={Uri.EscapeDataString(startAddr)}&end={Uri.EscapeDataString(endAddr)}";

                var response = await client.GetFromJsonAsync<ORSResponse>(url);

                if (response?.Features != null && response.Features.Count > 0)
                {
                    return response.Features[0].Properties.Segments[0].Distance;
                }
            }
            catch
            {
                // Silently fail to allow the controller to use the fallback dealer
            }
            return double.MaxValue;
        }
    }

    public class ORSResponse { public List<ORSFeature> Features { get; set; } = new(); }
    public class ORSFeature { public ORSProperty Properties { get; set; } = new(); }
    public class ORSProperty { public List<ORSSegment> Segments { get; set; } = new(); }
    public class ORSSegment { public double Distance { get; set; } }
}