using System.Net;
using Microsoft.EntityFrameworkCore;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var error = new ErrorDetails
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "Internal Server Error from the custom middleware.",
            DetailedMessage = exception.Message
        };

        // Custom Logic for specific Errors (like your SQL Constraint error)
        if (exception.InnerException?.Message.Contains("CK_Booking_VehicleYear") == true)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            error.StatusCode = (int)HttpStatusCode.BadRequest;
            error.Message = "Validation Error: Invalid Vehicle Year.";
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        return context.Response.WriteAsync(error.ToString());
    }
}