using Domain.Handlers.Commands;
using Domain.Handlers;
using Domain.Interfaces.Handlers;
using Domain.Interfaces.Messaging;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;
using Infrastructure.RabbitMq.Consumers;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces.Notification;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces.Storage;
using Infrastructure.RabbitMq.Publishers;
using Infrastructure.Storages;

namespace Infrastructure
{
    public static class Config
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMotoRepository, MotorcycleRepository>();
            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<NotifyOrderCouriersCommand>, NotifyOrderCouriersHandler>();
            services.AddSingleton<IPublisher<NotifyOrderCouriersCommand>, NotifyOrderCourierQueuePublisher>();
            services.AddHostedService<NotifyOrderCouriersQueueConsumer>();
        }
        public static void AddStorage(this IServiceCollection services)
        {
            services.AddScoped<IStorage, AmazonStorage>();
        }

        public static void ExecuteMigration(this IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (_db != null)
                {
                    if (_db.Database.GetPendingMigrations().Any())
                    {
                        _db.Database.Migrate();
                    }
                }
            }
        }
    }
}
