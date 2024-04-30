using Dominio.Handlers.Commands;
using Microsoft.Extensions.Configuration;

namespace Infraestrutura.RabbitMq.Consumers
{
    public class NotificarUsuariosQueuePublisher : RabbitMqPublisherBase<NotificacaoCommand>
    {
        private const string QUEUE_NAME = "Notificar_Entregadores_Ativos";
        private IServiceProvider _serviceProvider;

        public NotificarUsuariosQueuePublisher(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, QUEUE_NAME)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
