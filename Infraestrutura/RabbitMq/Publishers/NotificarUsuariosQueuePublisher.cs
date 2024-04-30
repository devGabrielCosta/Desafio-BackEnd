using Dominio.Handlers.Commands;
using Dominio.Interfaces.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

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
