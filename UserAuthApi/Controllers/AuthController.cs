using System.Data;
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            bool isValid = false;

            if (request.Role == "customer")
            {
                isValid = _context.Customers.Any(c => c.CMailId == request.Email && c.CPassword == request.Password);
            }
            
            else if (request.Role == "dealer")
            {
                // These must match the property names in DealerInfo.cs
                isValid = _context.Dealers.Any(d => d.DMailId == request.Email && d.DPassword == request.Password);
            }
            else if (request.Role == "admin")
            {
                // Replace with your Admin table logic
                isValid = _context.Admins.Any(a => a.AMailId == request.Email && a.APassword == request.Password);
            }

            if (isValid)
            {
                return Ok(new { success = true });
            }

            return Unauthorized(new { success = false, message = "Invalid credentials" });
        }
    }
}