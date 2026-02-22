using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetMyInventory()
        {
            // 1. Get the specific Dealer ID from the 'db_id' claim
            var currentDealerId = User.FindFirst("db_id")?.Value;

            if (string.IsNullOrEmpty(currentDealerId))
                return Unauthorized(new { message = "Dealer ID missing from token." });

            // 2. Fetch inventory asynchronously
            // The Global Middleware will catch any DB connectivity issues here
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

        // POST: api/DealerInventory/add-vehicle
        [HttpPost("add-vehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleInventory vehicle)
        {
            if (vehicle == null) return BadRequest();

            var dealerIdFromToken = User.FindFirst("db_id")?.Value;

            if (string.IsNullOrEmpty(dealerIdFromToken))
                return Unauthorized(new { message = "Dealer ID not found in token." });

            vehicle.DealerId = dealerIdFromToken;

            await _context.VehicleInventories.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Vehicle added successfully", vehicleId = vehicle.VehicleId });
        }

        // POST: api/DealerInventory/add-spare-part
        [HttpPost("add-spare-part")]
        public async Task<IActionResult> AddSparePart([FromBody] SparePartInventory sparePart)
        {
            if (sparePart == null) return BadRequest();

            // Force the DealerId to be the one from the JWT token
            sparePart.DealerId = User.FindFirst("db_id")?.Value;

            await _context.SparePartInventories.AddAsync(sparePart);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Spare part added successfully", sparePartId = sparePart.SparePartId });
        }
    }
}