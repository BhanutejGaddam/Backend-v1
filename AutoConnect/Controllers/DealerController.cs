using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for async EF Core extensions
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Net;
using System.Threading.Tasks; // Required for Task

[Route("api/[controller]")]
[ApiController]
public class DealerController : ControllerBase
{
    private readonly AppDbContext _context;
    public DealerController(AppDbContext context) { _context = context; }

    // ==========================================
    // WARRANTY SECTION
    // ==========================================

    [HttpGet("warranties")]
    public async Task<IActionResult> GetAllWarranties() => Ok(await _context.WarrantyCompliances.ToListAsync());

    [HttpGet("warranties/{vehicleNo}")]
    public async Task<IActionResult> GetWarrantyByNo(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = await _context.WarrantyCompliances.FindAsync(decodedNo);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost("warranties")]
    public async Task<IActionResult> CreateWarranty([FromBody] WarrantyCompliance data)
    {
        await _context.WarrantyCompliances.AddAsync(data);
        await _context.SaveChangesAsync();
        return Ok(data);
    }

    [HttpPut("warranties/{vehicleNo}")]
    public async Task<IActionResult> UpdateWarranty(string vehicleNo, [FromBody] WarrantyCompliance data)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var existing = await _context.WarrantyCompliances.FindAsync(decodedNo);
        if (existing == null) return NotFound();

        existing.Status = data.Status;
        existing.IssuedDate = data.IssuedDate;
        existing.ExpiryDate = data.ExpiryDate;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("warranties/{vehicleNo}")]
    public async Task<IActionResult> DeleteWarranty(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = await _context.WarrantyCompliances.FindAsync(decodedNo);

        if (item == null) return NotFound(new { message = "Warranty record not found" });

        // Remove remains synchronous because it only marks the entity state as 'Deleted' locally
        _context.WarrantyCompliances.Remove(item);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Warranty deleted successfully" });
    }

    // ==========================================
    // COMPLIANCE SECTION
    // ==========================================

    [HttpGet("compliance")]
    public async Task<IActionResult> GetAllCompliance() => Ok(await _context.ComplianceInformations.ToListAsync());

    [HttpGet("compliance/{vehicleNo}")]
    public async Task<IActionResult> GetComplianceByNo(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = await _context.ComplianceInformations.FindAsync(decodedNo);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost("compliance")]
    public async Task<IActionResult> CreateCompliance([FromBody] ComplianceInformation data)
    {
        await _context.ComplianceInformations.AddAsync(data);
        await _context.SaveChangesAsync();
        return Ok(data);
    }

    [HttpPut("compliance/{vehicleNo}")]
    public async Task<IActionResult> UpdateCompliance(string vehicleNo, [FromBody] ComplianceInformation data)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var existing = await _context.ComplianceInformations.FindAsync(decodedNo);
        if (existing == null) return NotFound();

        existing.PollutionCheck = data.PollutionCheck;
        existing.FitnessCheck = data.FitnessCheck;
        existing.RcCheck = data.RcCheck;
        existing.LastChecked = data.LastChecked;
        existing.Expiry = data.Expiry;

        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("compliance/{vehicleNo}")]
    public async Task<IActionResult> DeleteCompliance(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = await _context.ComplianceInformations.FindAsync(decodedNo);

        if (item == null) return NotFound(new { message = "Compliance record not found" });

        // Remove remains synchronous 
        _context.ComplianceInformations.Remove(item);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Compliance deleted successfully" });
    }
}