using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpRequestException ex) // Handle HttpClient errors from microservices
        {
            _logger.LogError(ex, "HttpRequestException occurred while communicating with microservices.");
            await HandleExceptionAsync(context, HttpStatusCode.BadGateway, ex.Message);
        }
        catch (ValidationException ex) // Handle validation exceptions
        {
            _logger.LogError(ex, "Validation error occurred.");
            await HandleExceptionAsync(context, HttpStatusCode.UnprocessableEntity, ex.Message);
        }
        catch (ArgumentException ex) // Handle argument exceptions
        {
            _logger.LogError(ex, "ArgumentException occurred.");
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex) // Handle all other exceptions
        {
            _logger.LogError(ex, "An unexpected exception occurred.");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}
