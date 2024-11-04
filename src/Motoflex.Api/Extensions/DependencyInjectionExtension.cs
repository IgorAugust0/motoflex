using Motoflex.Infrastructure;
using Motoflex.Application;

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
            // services.AddControllers().AddJsonOptions(options => { /* */});

            // TODO: Configure API Documentation
            //services.AddEndpointsApiExplorer();

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
