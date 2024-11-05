using Motoflex.Application.DTOs.Responses;

namespace Motoflex.Api.Extensions
{
    /// <summary>
    /// Middleware that catches all unhandled exceptions in the application
    /// and converts them to standardized JSON responses
    /// </summary>
    public sealed class GlobalExceptionMiddleware(
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment environment) : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _environment = environment;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Continue down the middleware pipeline
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "An error occurred: {Message}",
                    exception.Message);
                
                // Convert exception to HTTP response
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Map different exception types to appropriate HTTP status codes
            // Using pattern matching for cleaner syntax
            var (statusCode, message) = exception switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An error occurred")
            };

            // Set response type to JSON
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Create error response object
            // Include stack trace only in development environment
            var response = new ApiExceptionResponse
            {
                StatusCode = statusCode,
                Message = message,
                Details = _environment.IsDevelopment() ? exception.StackTrace : null
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
