using Motoflex.Infrastructure;
using Motoflex.Application;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

namespace Motoflex.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        private static readonly string[] OrderedGroups = ["motos", "entregadores", "locacao"];

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Motoflex API", Version = "v1" });
                // c.OrderActionsBy(apiDesc => Array.IndexOf(OrderedGroups, apiDesc.GroupName).ToString("D2")); // this custom sorting is not actually working
            });

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
