using Motoflex.Infrastructure;
using Motoflex.Application;
using System.Text.Json.Serialization;

namespace Motoflex.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Logging
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
                logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);
            });

            // TODO: Configure Authentication

            // TODO: Configure Controllers and JSON Options
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            // TODO: Configure API Documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Configure Database Context and Repositories
            services.AddPSQLContext(configuration);
            services.AddRepositories();

            // Configure Application Services and *Command Handlers
            services.AddServices();

            // TODO: Configure *Messaging and Storage
            //services.AddRabbitMq();
            services.AddAmazonStorage();
        }
    }
}
