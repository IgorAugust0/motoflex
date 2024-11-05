using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Motoflex.Api.Extensions
{
    /// <summary>
    /// Extensions for configuring Swagger with JWT authentication
    /// </summary>
    public static class SwaggerExtensions
    {
        private static readonly string[] OrderedGroups = ["motos", "entregadores", "locacao"];
        private const string SecurityScheme = "JWT";
        internal const string SecuritySchemaName = "Bearer";

        public static void AddSwaggerGenWithAuth(this IServiceCollection services, SwaggerOptions options)
        {
            services.AddSwaggerGen(config =>
            {
                // Basic configuration
                ConfigureBasicSettings(config, options);

                // Optional authentication
                if (options.EnableAuthentication) ConfigureAuthentication(config);
            });
        }

        private static void ConfigureBasicSettings(SwaggerGenOptions config, SwaggerOptions options)
        {
            // API Information
            config.SwaggerDoc(options.Version, new OpenApiInfo
            {
                Title = options.Title,
                Version = options.Version,
                Description = options.Description
            });

            // XML Documentation
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            config.IncludeXmlComments(xmlPath);

            // Endpoint Ordering (not actually working as intended)
            config.OrderActionsBy(apiDesc =>
                Array.IndexOf(OrderedGroups, apiDesc.GroupName).ToString("D2"));
            // config.OrderActionsBy(apiDesc => 
            //     Array.IndexOf(OrderedGroups, apiDesc.RelativePath?.Split('/')[1] ?? string.Empty).ToString("D2"));
        }

        private static void ConfigureAuthentication(SwaggerGenOptions config)
        {
            config.AddSecurityDefinition(SecuritySchemaName, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = SecuritySchemaName,
                BearerFormat = SecurityScheme,
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using Bearer scheme."
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecuritySchemaName
                        }
                    },
                    Array.Empty<string>()
                }
            });

            config.OperationFilter<AuthorizeOperationFilter>();
        }
    }

    /// <summary>
    /// Filter to add authorization requirements to Swagger operations
    /// </summary>
    internal sealed class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if action or controller has Authorize attribute
            var requiresAuthorization = context.MethodInfo.GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Any()
                || (context.MethodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any()).GetValueOrDefault(false); // ?? false);

            if (!requiresAuthorization) return;

            // Add security requirement
            operation.Security =
            [
                new()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = SwaggerExtensions.SecuritySchemaName
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            ];
        }
    }
}
