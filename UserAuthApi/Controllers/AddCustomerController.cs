using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Models;
using UserAuthApi.Data; // Ensure this points to your DbContext namespace

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddCustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 1. Map DTO to the actual CustomerInfo Model
                var newCustomer = new CustomerInfo
                {
                    // Generate a unique ID (e.g., CUS0001)
                    CustomerId = await GenerateCustomerId(),
                    CFirstName = dto.c_first_name,
                    CMiddleName = dto.c_middle_name,
                    CLastName = dto.c_last_name,
                    CUsername = dto.c_username,
                    CPassword = dto.c_password, // Note: In a real app, hash this!
                    CMailId = dto.c_mail_id,
                    CContactInfo = dto.c_contact_info,
                    CAddress = dto.c_address,
                    VehicleModelYear = dto.vehicle_model_year,
                    PurchaseDate = dto.purchase_date,
                    AddedByDealer = dto.dealer_id,
                    LoyaltyPoints = 100 // Default for new customers
                };

                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Customer added successfully", id = newCustomer.CustomerId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<string> GenerateCustomerId()
        {
            // Simple logic: count existing and increment, or use a Guid substring
            var count = await _context.Customers.CountAsync();
            return $"CUS{(count + 1):D4}"; // Returns CUS0001, CUS0002, etc.
        }
    }

    // DTO to match your Angular Form exactly
    public class CustomerRegistrationDto
    {
        public required string c_first_name { get; set; }
        public required string c_middle_name { get; set; }
        public required string c_last_name { get; set; }
        public required string c_username { get; set; }
        public required string c_password { get; set; }
        public required string c_mail_id { get; set; }
        public required long c_contact_info { get; set; }
        public required string c_address { get; set; }
        public required string vehicle_model_year { get; set; }
        public required DateTime purchase_date { get; set; }
        public required string dealer_id { get; set; }
    }
}