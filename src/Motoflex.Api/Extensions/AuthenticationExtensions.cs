using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.IdentityModel.JsonWebTokens; // newer version that will be switched to

namespace Motoflex.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind and validate JWT configuration section
            var jwtOptions = configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT configuration is missing");

            // Validate minimum security requirements
            if (string.IsNullOrEmpty(jwtOptions.Secret))
                throw new InvalidOperationException("JWT secret is not configured");

            if (jwtOptions.Secret.Length < 16)
                throw new InvalidOperationException("JWT secret must be at least 16 characters long");

            // Register strongly typed options for dependency injection
            services.Configure<JwtOptions>(
                configuration.GetSection(JwtOptions.SectionName));

            // Configure JWT authentication scheme
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Configure HTTPS requirement
                options.RequireHttpsMetadata = jwtOptions.RequireHttpsMetadata;
                options.SaveToken = true;

                // Configure token validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Enable validation of issuer and audience
                    ValidateIssuer = true,
                    ValidateAudience = true,

                    // Set valid issuer and audience from configuration
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,

                    // Configure signing key from secret
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret)),

                    // Validate token expiration and signature
                    ValidateLifetime = jwtOptions.ValidateLifetime,
                    ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,

                    // Add lifetime validation
                    LifetimeValidator = (notBefore, expires, token, parameters) =>
                    {
                        if (expires == null) return false;

                        // Validate token hasn't expired and is not valid too far in the future
                        return expires > DateTime.UtcNow &&
                               expires <= DateTime.UtcNow.AddHours(jwtOptions.ExpiryInHours);
                    },

                    // Remove clock skew tolerance
                    ClockSkew = TimeSpan.Zero
                };

                // Configure JWT bearer events
                options.Events = new JwtBearerEvents
                {
                    // Add custom header when token is expired
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
