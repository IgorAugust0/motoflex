namespace Motoflex.Application.DTOs.Responses
{
    /// <summary>
    /// Represents a standardized error response returned by the API
    /// </summary>
    public record ApiExceptionResponse
    {
        public int StatusCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public string? Details { get; init; }
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    }
}
