using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Motoflex.Domain.Interfaces.Repositories;
using Motoflex.Infrastructure.Contexts;
using Motoflex.Infrastructure.Repositories;

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
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMotoRepository, MotorcycleRepository>();
            services.AddScoped<IRenterRepository, RenterRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
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
