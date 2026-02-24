using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Enforces JWT verification for all requests
    public class AnalyticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnalyticsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("sales-report/{dealerId}")]
        public async Task<IActionResult> GetSalesReport(string dealerId)
        {
            // --- Security Verification ---
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdFromToken = User.FindFirst("db_id")?.Value;

            // Prevent a dealer from accessing another dealer's sales data
            if (userRole == "dealer" && userIdFromToken != dealerId)
            {
                return Forbid();
            }

            // --- 1. Vehicle Sales Metrics ---
            var salesList = await _context.VehicleSalesInfo
                .Where(s => s.DealerId == dealerId)
                .ToListAsync();

            var vehiclesSold = salesList.Count;
            var salesRevenue = salesList.Sum(s => s.Price);

            // --- 2. Inventory Metrics ---
            var stockRemaining = await _context.VehicleInventories
                .Where(i => i.DealerId == dealerId && i.IsAvailable == true)
                .SumAsync(i => i.AvailableUnits ?? 0);

            // --- 3. Service Workshop Metrics ---
            var serviceBookings = await _context.Bookings
                .Where(b => b.Selected_Dealer_Id == dealerId && b.BookingStatus == "Completed")
                .ToListAsync();

            var servicesProcessed = serviceBookings.Count;
            var serviceRevenue = serviceBookings.Sum(b => b.TotalBill ?? 0);

            // --- 4. Calculations (Assuming 10% Sales Margin, 30% Service Margin) ---
            var salesProfit = salesRevenue * 0.10m;
            var serviceProfit = serviceRevenue * 0.30m;

            return Ok(new
            {
                reportID = $"RPT-{dealerId.ToUpper()}-{DateTime.Now:yyyyMMdd}",
                generatedDate = DateTime.UtcNow,
                metrics = new
                {
                    vehiclesSold,
                    vehiclesRemaining = stockRemaining,
                    salesRevenue,
                    salesProfit,
                    serviceRequestsProcessed = servicesProcessed,
                    serviceCapacity = 20, // Business logic constant
                    serviceRevenue,
                    serviceProfit,
                    netDealerProfit = salesProfit + serviceProfit
                }
            });
        }

        [HttpGet("revenue-trends/{dealerId}")]
        public async Task<IActionResult> GetRevenueTrends(string dealerId)
        {
            // Security Check
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdFromToken = User.FindFirst("db_id")?.Value;
            if (userRole == "dealer" && userIdFromToken != dealerId) return Forbid();

            var last6Months = DateTime.UtcNow.AddMonths(-5);

            // Get Sales Trends
            var salesTrend = await _context.VehicleSalesInfo
                .Where(s => s.DealerId == dealerId && s.SoldDate >= last6Months)
                .ToListAsync();

            // Get Service Trends
            var serviceTrend = await _context.Bookings
                .Where(b => b.Selected_Dealer_Id == dealerId && b.Slot >= last6Months && b.BookingStatus == "Completed")
                .ToListAsync();

            // Grouping and Formatting for the Chart
            var monthlyData = Enumerable.Range(0, 6)
                .Select(i => DateTime.UtcNow.AddMonths(-i))
                .Select(date => new
                {
                    month = date.ToString("MMM yyyy"),
                    salesRevenue = salesTrend
                        .Where(s => s.SoldDate.Month == date.Month && s.SoldDate.Year == date.Year)
                        .Sum(s => s.Price),
                    serviceRevenue = serviceTrend
                        .Where(b => b.Slot.Month == date.Month && b.Slot.Year == date.Year)
                        .Sum(b => b.TotalBill ?? 0)
                })
                .Reverse()
                .ToList();

            return Ok(new { dealerSales = monthlyData });
        }
    }
}