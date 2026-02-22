using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using UserAuthApi.Models;
using UserAuthApi.Data;
using System.IdentityModel.Tokens.Jwt;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "dealer")] // Secure with JWT
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

            // Get Dealer ID from JWT Claim
            var dealerIdFromToken = User.FindFirst("db_id")?.Value;
            if (string.IsNullOrEmpty(dealerIdFromToken))
                return Unauthorized(new { message = "Dealer ID not found in token." });

            // Start a transaction to ensure all-or-nothing insertion
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Generate Customer ID
                string newCustomerId = await GenerateCustomerId();
                string newVehicleNUmber = await GenerateRandomVehicleNumber();

                // 2. Map & Add CustomerInfo
                var newCustomer = new CustomerInfo
                {
                    CustomerId = newCustomerId,
                    CFirstName = dto.c_first_name,
                    CMiddleName = dto.c_middle_name,
                    CLastName = dto.c_last_name,
                    CUsername = dto.c_username,
                    CPassword = dto.c_password,
                    CMailId = dto.c_mail_id,
                    CContactInfo = dto.c_contact_info,
                    CAddress = dto.c_address,
                    VehicleModelYear = dto.vehicle_model_year,
                    PurchaseDate = dto.purchase_date,
                    AddedByDealer = dealerIdFromToken,
                    LoyaltyPoints = 100
                };
                _context.Customers.Add(newCustomer);

                // 3. Add VehicleSalesInfo
                var salesEntry = new VehicleSalesInfo
                {
                    DealerId = dealerIdFromToken,
                    CustomerId = newCustomerId,
                    VehicleSold = dto.vehicle_model_year, // Or a specific model property
                    SoldDate = dto.purchase_date,
                    Price = dto.price // Update this if you add a price field to your DTO
                };
                _context.VehicleSalesInfo.Add(salesEntry);

                // 4. Add WarrantyCompliance
                var warrantyEntry = new WarrantyCompliance
                {
                    VehicleNumber = newVehicleNUmber, // Default as requested
                    Status = "Active",
                    IssuedDate = dto.purchase_date,
                    ExpiryDate = dto.purchase_date.AddYears(1), // Example: 1 year warranty
                    dealerId = dealerIdFromToken,
                    customer_id=newCustomerId
                };
                _context.WarrantyCompliances.Add(warrantyEntry);

                // 5. Add ComplianceInformation
                var complianceEntry = new ComplianceInformation
                {
                    VehicleNumber = newVehicleNUmber, // Default as requested
                    PollutionCheck = "Checked",
                    FitnessCheck = "Checked",
                    RcCheck = "Checked",
                    LastChecked = dto.purchase_date,
                    Expiry = dto.purchase_date.AddMonths(6), // 6 months after purchase
                    dealerId = dealerIdFromToken,
                    customer_id = newCustomerId
                };
                _context.ComplianceInformations.Add(complianceEntry);

                // Save all changes and commit transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Customer and all compliance records added successfully",
                    customerId = newCustomerId
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // This sends the inner exception (the real reason) back to Angular
                var innerError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new { message = innerError });
            }
        }

        private async Task<string> GenerateCustomerId()
        {
            var count = await _context.Customers.CountAsync();
            return $"CUS{(count + 1):D4}";
        }

        private async Task<string> GenerateRandomVehicleNumber()
        {
            var uniqueVehicleNum = "TN-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            return uniqueVehicleNum;
        }
    }

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

        public required long price { get; set; }
        // Note: dealer_id is now handled by the JWT claim
    }
}