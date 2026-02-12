using System.Data;
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

        private string GenerateToken(string email, string role)
        {
            // 1. Create the security key from appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 2. Define the user's "Claims"
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Role, role), // This allows [Authorize(Roles = "admin")]
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique ID for the token
    };

            // 3. Create the token object
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3), // Token expires in 3 hours
                signingCredentials: credentials);

            // 4. Serialize the token into a string
            return new JwtSecurityTokenHandler().WriteToken(token);
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
            // 1. Identify and Validate the User
            string userRole = "";
            bool isValid = false;

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

            // 2. If valid, generate and return the token
            if (isValid)
            {
                var token = GenerateToken(request.Email, userRole);
                return Ok(new
                {
                    success = true,
                    token = token,
                    message = "Login successful",
                    role = userRole // Helpful for frontend routing
                });
            }

            return Unauthorized(new { success = false, message = "Invalid email, password, or role" });
        }
    }
}