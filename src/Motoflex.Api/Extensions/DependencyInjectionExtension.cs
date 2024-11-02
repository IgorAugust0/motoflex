using Motoflex.Infrastructure;

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

            // Configure Database Context and Repositories
            services.AddPSQLContext(configuration);
            services.AddRepositories();
        }
    }
}
