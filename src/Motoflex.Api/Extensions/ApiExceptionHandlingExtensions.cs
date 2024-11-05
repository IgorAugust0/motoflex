namespace Motoflex.Api.Extensions
{
     /// <summary>
    /// Extension methods to add error handling middleware to the application pipeline
    /// </summary>
    public static class ApiExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandling(
            this IApplicationBuilder app)
            => app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}
