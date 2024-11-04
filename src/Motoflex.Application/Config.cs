using Microsoft.Extensions.DependencyInjection;
using Motoflex.Application.Interfaces;
using Motoflex.Application.Services;

namespace Motoflex.Application
{
    public static class Config
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IRenterService, RenterService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IOrderService, OrderService>();
        }

        // TODO: Add command handlers here
    }
}
