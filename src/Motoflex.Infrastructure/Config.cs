using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Motoflex.Domain.Interfaces.Notifications;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Domain.Interfaces.Storage;
using Motoflex.Infrastructure.Contexts;
using Motoflex.Infrastructure.Repositories;
using Motoflex.Infrastructure.Storage;

namespace Motoflex.Infrastructure
{
    public static class Config
    {
        public static void AddPSQLContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Connection String: {connectionString}"); // Debug print

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMotoRepository, MotorcycleRepository>();
            services.AddScoped<IRenterRepository, RenterRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static void AddAmazonStorage(this IServiceCollection services)
        {
            services.AddScoped<IStorage, AmazonStorage>();
        }

        public static void ExecuteMigrations(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var isPending = db.Database.GetPendingMigrations().Any();
            if (isPending) db.Database.Migrate();
        }
    }
}
