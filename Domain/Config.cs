using Domain.Handlers.Commands;
using Domain.Handlers;
using Domain.Interfaces.Handlers;
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

        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<NotifyOrderCouriersCommand>, NotifyOrderCouriersHandler>();
        }
    }
}
