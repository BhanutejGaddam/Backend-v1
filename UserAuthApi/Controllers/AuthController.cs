using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CustomerInfo customer) // Change User to CustomerInfo
        {
            if (customer == null) return BadRequest();

            _context.Customers.Add(customer); // Change _context.Users to _context.Customers
            _context.SaveChanges();

            return Ok(new { message = "Registration Successful" });
        }
    }
}