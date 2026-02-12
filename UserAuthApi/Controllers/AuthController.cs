using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CustomerInfo customer)
        {
            if (customer == null) return BadRequest();

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return Ok(new { message = "Registration Successful" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string userRole = "";
            bool isValid = false;

            // Simple validation logic (Reminder: Use hashing for production!)
            if (request.Role == "customer")
            {
                isValid = _context.Customers.Any(c => c.CMailId == request.Email && c.CPassword == request.Password);
                userRole = "customer";
            }
            else if (request.Role == "dealer")
            {
                isValid = _context.Dealers.Any(d => d.DMailId == request.Email && d.DPassword == request.Password);
                userRole = "dealer";
            }
            else if (request.Role == "admin")
            {
                isValid = _context.Admins.Any(a => a.AMailId == request.Email && a.APassword == request.Password);
                userRole = "admin";
            }

            if (isValid)
            {
                var token = GenerateToken(request.Email, userRole);
                return Ok(new
                {
                    success = true,
                    token = token,
                    role = userRole
                });
            }

            return Unauthorized(new { success = false, message = "Invalid credentials" });
        }

        private string GenerateToken(string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.Role, role), // For [Authorize(Roles="admin")]
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}