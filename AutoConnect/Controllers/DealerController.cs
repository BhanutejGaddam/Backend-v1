using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Data;
using UserAuthApi.Models;
using System.Net;

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
    public IActionResult GetAllWarranties() => Ok(_context.WarrantyCompliances.ToList());

    [HttpGet("warranties/{vehicleNo}")]
    public IActionResult GetWarrantyByNo(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = _context.WarrantyCompliances.Find(decodedNo);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost("warranties")]
    public IActionResult CreateWarranty([FromBody] WarrantyCompliance data)
    {
        _context.WarrantyCompliances.Add(data);
        _context.SaveChanges();
        return Ok(data);
    }

    [HttpPut("warranties/{vehicleNo}")]
    public IActionResult UpdateWarranty(string vehicleNo, [FromBody] WarrantyCompliance data)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var existing = _context.WarrantyCompliances.Find(decodedNo);
        if (existing == null) return NotFound();

        existing.Status = data.Status;
        existing.IssuedDate = data.IssuedDate;
        existing.ExpiryDate = data.ExpiryDate;

        _context.SaveChanges();
        return Ok(existing);
    }

    [HttpDelete("warranties/{vehicleNo}")]
    public IActionResult DeleteWarranty(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = _context.WarrantyCompliances.Find(decodedNo);

        if (item == null) return NotFound(new { message = "Warranty record not found" });

        _context.WarrantyCompliances.Remove(item);
        _context.SaveChanges();
        return Ok(new { message = "Warranty deleted successfully" });
    }

    // ==========================================
    // COMPLIANCE SECTION
    // ==========================================

    [HttpGet("compliance")]
    public IActionResult GetAllCompliance() => Ok(_context.ComplianceInformations.ToList());

    [HttpGet("compliance/{vehicleNo}")]
    public IActionResult GetComplianceByNo(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = _context.ComplianceInformations.Find(decodedNo);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost("compliance")]
    public IActionResult CreateCompliance([FromBody] ComplianceInformation data)
    {
        _context.ComplianceInformations.Add(data);
        _context.SaveChanges();
        return Ok(data);
    }

    [HttpPut("compliance/{vehicleNo}")]
    public IActionResult UpdateCompliance(string vehicleNo, [FromBody] ComplianceInformation data)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var existing = _context.ComplianceInformations.Find(decodedNo);
        if (existing == null) return NotFound();

        existing.PollutionCheck = data.PollutionCheck;
        existing.FitnessCheck = data.FitnessCheck;
        existing.RcCheck = data.RcCheck;
        existing.LastChecked = data.LastChecked;
        existing.Expiry = data.Expiry;

        _context.SaveChanges();
        return Ok(existing);
    }

    [HttpDelete("compliance/{vehicleNo}")]
    public IActionResult DeleteCompliance(string vehicleNo)
    {
        var decodedNo = WebUtility.UrlDecode(vehicleNo);
        var item = _context.ComplianceInformations.Find(decodedNo);

        if (item == null) return NotFound(new { message = "Compliance record not found" });

        _context.ComplianceInformations.Remove(item);
        _context.SaveChanges();
        return Ok(new { message = "Compliance deleted successfully" });
    }
}