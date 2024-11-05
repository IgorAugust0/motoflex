namespace Motoflex.Api.Extensions
{
    /// <summary>
    /// Strongly typed options class for JWT configuration
    /// </summary>
    public sealed class JwtOptions
    {
        // Name of the configuration section in appsettings.json
        public const string SectionName = "JWT";

        // Required JWT configuration properties with init-only setters
        public required string Secret { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }

        // Optional properties with default values
        public int ExpiryInHours { get; init; } = 1;
        public int ExpiryInMinutes { get; init; } = 60;
        public bool RequireHttpsMetadata { get; init; } = true;
        public bool ValidateLifetime { get; init; } = true;
        public bool ValidateIssuerSigningKey { get; init; } = true;
    }
}
