using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? DetailedMessage { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}


