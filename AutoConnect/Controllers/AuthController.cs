using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks; // Required for Task
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for async EF Core extensions
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
        public async Task<IActionResult> Register([FromBody] CustomerInfo customer) // Changed to async Task
        {
            if (customer == null) return BadRequest();

            // Replaced Add with AddAsync and SaveChanges with SaveChangesAsync
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration Successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) // Changed to async Task
        {
            string userRole = "";
            string userId = ""; // This will hold DId, CId, or AId
            bool isValid = false;

            if (request.Role == "customer")
            {
                // Replaced FirstOrDefault with FirstOrDefaultAsync
                var user = await _context.Customers.FirstOrDefaultAsync(c => c.CMailId == request.Email && c.CPassword == request.Password);
                if (user != null) { isValid = true; userRole = "customer"; userId = user.CustomerId; }
            }
            else if (request.Role == "dealer")
            {
                // Replaced FirstOrDefault with FirstOrDefaultAsync
                var user = await _context.Dealers.FirstOrDefaultAsync(d => d.DMailId == request.Email && d.DPassword == request.Password);
                if (user != null) { isValid = true; userRole = "dealer"; userId = user.DealerId; }
            }
            else if (request.Role == "admin")
            {
                // Replaced FirstOrDefault with FirstOrDefaultAsync
                var user = await _context.Admins.FirstOrDefaultAsync(a => a.AMailId == request.Email && a.APassword == request.Password);
                if (user != null) { isValid = true; userRole = "admin"; userId = user.AdminId; }
            }

            if (isValid)
            {
                // We pass the specific Database ID into the token generator
                // (Token generation is CPU-bound, so it remains synchronous)
                var token = GenerateToken(request.Email, userRole, userId);
                return Ok(new
                {
                    success = true,
                    token = token,
                    role = userRole,
                    id = userId
                });
            }

            return Unauthorized(new { success = false, message = "Invalid credentials" });
        }

        private string GenerateToken(string email, string role, string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email), // Email remains the primary 'subject'
                new Claim(ClaimTypes.Role, role),              // Role for [Authorize(Roles="dealer")]
                new Claim("db_id", userId),                    // The specific DId, CId, or AId
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