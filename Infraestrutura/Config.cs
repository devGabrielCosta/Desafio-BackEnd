using Dominio.Handlers.Commands;
using Dominio.Handlers;
using Dominio.Interfaces.Handlers;
using Dominio.Interfaces.Mensageria;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;
using Infraestrutura.RabbitMq.Consumers;
using Infraestrutura.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Dominio.Interfaces.Notification;
using Infraestrutura.Notification;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura
{
    public static class Config
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddScoped<IEntregadorRepository, EntregadorRepository>();
            services.AddScoped<ILocacaoRepository, LocacaoRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
        }

        public static void AddRabbitMq(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<NotificacaoCommand>, NotificacaoHandler>();
            services.AddSingleton<IPublisher<NotificacaoCommand>, NotificarUsuariosQueuePublisher>();
            services.AddHostedService<NotificarUsuariosQueueConsumer>();
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
