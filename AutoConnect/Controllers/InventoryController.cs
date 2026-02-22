using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System;
using System.Linq;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("{dealerId}")]
        public IActionResult GetDealerInventory(string dealerId)
        {
            try
            {
                var vehicles = _context.VehicleInventories
                    .Where(v => v.DealerId == dealerId)
                    .ToList();

                var spareParts = _context.SparePartInventories
                    .Where(p => p.DealerId == dealerId)
                    .ToList();

                return Ok(new
                {
                    Vehicles = vehicles,
                    SpareParts = spareParts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving inventory", error = ex.Message });
            }
        }
    }
}