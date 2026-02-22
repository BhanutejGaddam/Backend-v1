using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for async EF Core extensions
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Security.Claims;
using System.Threading.Tasks; // Required for Task
using System;
using System.Linq;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "dealer")]
    public class DealerInventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DealerInventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DealerInventory/my-inventory
        [HttpGet("my-inventory")]
        [Authorize(Roles = "dealer")]
        public async Task<IActionResult> GetMyInventory() // Changed to async Task
        {
            try
            {
                // 1. Get the specific Dealer ID from the 'db_id' claim
                var currentDealerId = User.FindFirst("db_id")?.Value;

                if (string.IsNullOrEmpty(currentDealerId))
                    return Unauthorized(new { message = "Dealer ID missing from token." });

                // 2. Fetch inventory directly using that ID asynchronously
                // Replaced ToList() with ToListAsync()
                var vehicles = await _context.VehicleInventories
                    .Where(v => v.DealerId == currentDealerId)
                    .ToListAsync();

                var spareParts = await _context.SparePartInventories
                    .Where(p => p.DealerId == currentDealerId)
                    .ToListAsync();

                return Ok(new
                {
                    DealerId = currentDealerId,
                    Vehicles = vehicles,
                    SpareParts = spareParts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server Error", error = ex.Message });
            }
        }

        // POST: api/DealerInventory/add-vehicle
        [HttpPost("add-vehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleInventory vehicle) // Changed to async Task
        {
            if (vehicle == null) return BadRequest();

            // Change ClaimTypes.NameIdentifier to "db_id"
            var dealerIdFromToken = User.FindFirst("db_id")?.Value;

            if (string.IsNullOrEmpty(dealerIdFromToken))
            {
                return Unauthorized(new { message = "Dealer ID not found in token." });
            }

            vehicle.DealerId = dealerIdFromToken;

            // Replaced Add with AddAsync and SaveChanges with SaveChangesAsync
            await _context.VehicleInventories.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Vehicle added successfully", vehicleId = vehicle.VehicleId });
        }

        // POST: api/DealerInventory/add-spare-part
        [HttpPost("add-spare-part")]
        public async Task<IActionResult> AddSparePart([FromBody] SparePartInventory sparePart) // Changed to async Task
        {
            if (sparePart == null) return BadRequest();

            // Force the DealerId to be the one from the JWT token
            sparePart.DealerId = User.FindFirst("db_id")?.Value;

            // Replaced Add with AddAsync and SaveChanges with SaveChangesAsync
            await _context.SparePartInventories.AddAsync(sparePart);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Spare part added successfully", sparePartId = sparePart.SparePartId });
        }
    }
}