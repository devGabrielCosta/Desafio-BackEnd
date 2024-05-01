using Dominio.Handlers.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infraestrutura.RabbitMq.Consumers
{
    public class NotificarUsuariosQueuePublisher : RabbitMqPublisherBase<NotificacaoCommand>
    {
        private const string QUEUE_NAME = "Notificar_Entregadores_Ativos";
        private IServiceProvider _serviceProvider;

        public NotificarUsuariosQueuePublisher(
            IConfiguration configuration, 
            IServiceProvider serviceProvider, 
            ILogger<NotificarUsuariosQueuePublisher> logger
            ) : base(configuration, QUEUE_NAME, logger)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
