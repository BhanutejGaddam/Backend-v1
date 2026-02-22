using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetDealerInventory(string dealerId)
        {
            // Global Middleware catches any errors during these DB queries
            var vehicles = await _context.VehicleInventories
                .Where(v => v.DealerId == dealerId)
                .ToListAsync();

            var spareParts = await _context.SparePartInventories
                .Where(p => p.DealerId == dealerId)
                .ToListAsync();

            return Ok(new
            {
                Vehicles = vehicles,
                SpareParts = spareParts
            });
        }
    }
}