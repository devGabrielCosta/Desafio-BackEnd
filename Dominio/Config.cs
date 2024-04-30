using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Services;
using Dominio.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dominio
{
    public static class Config
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMotoService, MotoService>();
            services.AddScoped<IEntregadorService, EntregadorService>();
            services.AddScoped<ILocacaoService, LocacaoService>();
            services.AddScoped<IPedidoService, PedidoService>();
        }
    }
}
