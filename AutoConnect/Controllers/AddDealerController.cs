using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UserAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDealerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddDealerController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/AddDealer
        [HttpGet]
        public async Task<IActionResult> GetAllDealers()
        {
            // Middleware catches any connection or query errors
            var dealersList = await _context.Dealers.ToListAsync();
            return Ok(dealersList);
        }

        // POST: api/AddDealer/add
        [HttpPost("add")]
        public async Task<IActionResult> RegisterDealer([FromBody] DealerPostDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid data received" });

            // 1. Convert dPhone (string) to long (bigint)
            if (!long.TryParse(dto.DPhone, out long phoneAsLong))
            {
                return BadRequest(new { message = "Phone number must be numeric for bigint storage." });
            }

            // 2. Map the DTO to your AddDealer Model
            var newDealer = new AddDealer
            {
                DealerId = dto.DealerId,
                DFirstName = dto.DFirstName,
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

            // 3. Check for duplicates
            if (await _context.Dealers.AnyAsync(d => d.DealerId == newDealer.DealerId))
            {
                return Conflict(new { message = "A dealer with this ID already exists." });
            }

            // 4. Save to Database
            // If anything fails here, Middleware returns the clean JSON error
            await _context.Dealers.AddAsync(newDealer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dealer added successfully", id = newDealer.DealerId });
        }
    }

    public class DealerPostDto
    {
        public string DealerId { get; set; } = string.Empty;
        public string DFirstName { get; set; } = string.Empty;
        public string? DMiddleName { get; set; }
        public string? DLastName { get; set; }
        public string DMailId { get; set; } = string.Empty;
        public string DPhone { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public string DPassword { get; set; } = string.Empty;
        public string StoreAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string DUsername { get; set; } = string.Empty;
    }
}