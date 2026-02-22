using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Security.Claims;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "dealer")]
    public class DealerCustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DealerCustomersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("my-customers")]
        public async Task<IActionResult> GetMyCustomers()
        {
            // Extract the Dealer ID from the JWT 'db_id' claim
            var currentDealerId = User.FindFirst("db_id")?.Value;

            if (string.IsNullOrEmpty(currentDealerId))
                return Unauthorized(new { message = "Dealer identity not found in token." });

            // Fetch customers where AddedByDealer matches the ID from the token
            // Middleware will handle potential database connection errors automatically
            var customers = await _context.Customers
                .Where(c => c.AddedByDealer == currentDealerId)
                .OrderByDescending(c => c.PurchaseDate)
                .ToListAsync();

            return Ok(customers);
        }
    }
}