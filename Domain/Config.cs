using Domain.Interfaces.Services;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class Config
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<ICourierService, CourierService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}
