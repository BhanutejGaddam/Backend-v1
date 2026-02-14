using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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
        public IActionResult GetMyInventory()
        {
            try
            {
                // 1. Get the specific Dealer ID from the 'db_id' claim
                var currentDealerId = User.FindFirst("db_id")?.Value;

                if (string.IsNullOrEmpty(currentDealerId))
                    return Unauthorized(new { message = "Dealer ID missing from token." });

                // 2. Fetch inventory directly using that ID
                var vehicles = _context.VehicleInventories
                    .Where(v => v.DealerId == currentDealerId)
                    .ToList();

                var spareParts = _context.SparePartInventories
                    .Where(p => p.DealerId == currentDealerId)
                    .ToList();

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
        public IActionResult AddVehicle([FromBody] VehicleInventory vehicle)
        {
            if (vehicle == null) return BadRequest();

            // Force the DealerId to be the one from the JWT token for security
            vehicle.DealerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            _context.VehicleInventories.Add(vehicle);
            _context.SaveChanges();

            return Ok(new { message = "Vehicle added successfully", vehicleId = vehicle.VehicleId });
        }

        // POST: api/DealerInventory/add-spare-part
        [HttpPost("add-spare-part")]
        public IActionResult AddSparePart([FromBody] SparePartInventory sparePart)
        {
            if (sparePart == null) return BadRequest();

            // Force the DealerId to be the one from the JWT token
            sparePart.DealerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            _context.SparePartInventories.Add(sparePart);
            _context.SaveChanges();

            return Ok(new { message = "Spare part added successfully", sparePartId = sparePart.SparePartId });
        }
    }
}