using Motoflex.Infrastructure;
using Motoflex.Application;
using System.Text.Json.Serialization;

namespace Motoflex.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureLogging(services);
            ConfigureAuthentication(services, configuration);
            ConfigureApi(services);
            ConfigureSwagger(services, configuration);
            ConfigureInfrastructure(services, configuration);
        }

        private static void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
                logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
            });
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>()
                .Bind(configuration.GetSection(JwtOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddJwtAuthentication(configuration);
        }

        private static void ConfigureApi(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });
        }

        private static void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
        {
            // to enable authentication, set the value to true in appsettings.json
            var swaggerOptions = new SwaggerOptions
            {
                EnableAuthentication = configuration.GetValue("Swagger:EnableAuthentication", defaultValue: true)
            };
            // var swaggerOptions = configuration.GetSection(SwaggerOptions.SectionName).Get<SwaggerOptions>() ?? new SwaggerOptions();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithAuth(swaggerOptions);
        }

        private static void ConfigureInfrastructure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddPSQLContext(configuration);
            services.AddRepositories();
            services.AddServices();
            // services.AddRabbitMq();
            services.AddAmazonStorage();
        }
    }
}
