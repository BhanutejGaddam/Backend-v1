using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDealerController : ControllerBase
    {
        // Marking as readonly and adding a null-check in the constructor 
        // resolves the "'_context' is not null here" analyzer message.
        private readonly AppDbContext _context;

        public AddDealerController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Dealers
        [HttpGet]
        public IActionResult GetAllDealers()
        {
            try
            {
                // Returns the list from the Dealer_info table
                var dealersList = _context.Dealers.ToList();
                return Ok(dealersList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching dealers", error = ex.Message });
            }
        }

        // POST: api/Dealers/add
        [HttpPost("add")]
        public IActionResult RegisterDealer([FromBody] DealerPostDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid data received" });

            // 1. Convert dPhone (string) to long (bigint) to match your AddDealer model
            // This handles the conversion safely before reaching the database.
            if (!long.TryParse(dto.DPhone, out long phoneAsLong))
            {
                return BadRequest(new { message = "Phone number must be numeric for bigint storage." });
            }

            // 2. Map the DTO to your AddDealer Model
            var newDealer = new AddDealer
            {
                DealerId = dto.DealerId,
                DFirstName = dto.DFirstName,
                // Ensure these are NOT NULL even if the user leaves them blank in Angular
                DMiddleName = string.IsNullOrWhiteSpace(dto.DMiddleName) ? "N/A" : dto.DMiddleName,
                DLastName = string.IsNullOrWhiteSpace(dto.DLastName) ? "N/A" : dto.DLastName,
                DMailId = dto.DMailId,
                DContactInfo = phoneAsLong,
                StoreName = dto.StoreName,
                DPassword = dto.DPassword,
                StoreAddress = dto.StoreAddress,
                City = dto.City,
                State = dto.State,
                DUsername = dto.DUsername
            };

            try
            {
                // 3. Check for Duplicate Primary Key to prevent SQL errors
                if (_context.Dealers.Any(d => d.DealerId == newDealer.DealerId))
                {
                    return Conflict(new { message = "A dealer with this ID already exists." });
                }

                _context.Dealers.Add(newDealer);
                _context.SaveChanges();

                return Ok(new { message = "Dealer added successfully", id = newDealer.DealerId });
            }
            catch (Exception ex)
            {
                // ex.InnerException usually contains the real SQL error (e.g., "Cannot insert duplicate key")
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { message = "Database operation failed", error = innerMessage });
            }
        }
    }

    /// <summary>
    /// This DTO acts as a bridge between your Angular Frontend and your SQL Model.
    /// It receives the phone number as a string to prevent binding errors.
    /// </summary>
    public class DealerPostDto
    {
        public string DealerId { get; set; } = string.Empty;
        public string DFirstName { get; set; } = string.Empty;
        public string? DMiddleName { get; set; }
        public string? DLastName { get; set; }
        public string DMailId { get; set; } = string.Empty;
        public string DPhone { get; set; } = string.Empty; // Received as string from Angular
        public string StoreName { get; set; } = string.Empty;
        public string DPassword { get; set; } = string.Empty;
        public string StoreAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string DUsername { get; set; } = string.Empty;
    }
}