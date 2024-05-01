using Dominio.Handlers.Commands;
using Dominio.Interfaces.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infraestrutura.RabbitMq.Consumers
{
    public class NotificarUsuariosQueueConsumer : RabbitMqConsumerBase<NotificacaoCommand>
    {
        private const string QUEUE_NAME = "Notificar_Entregadores_Ativos";
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public NotificarUsuariosQueueConsumer(
            IConfiguration configuration, 
            IServiceProvider serviceProvider,
            ILogger<NotificarUsuariosQueueConsumer> logger
        ) : base(configuration, QUEUE_NAME, logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override void OnExecute(NotificacaoCommand message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<ICommandHandler<NotificacaoCommand>>();

                if (handler == null)
                    throw new NotImplementedException();

                handler.Handle(message);
            }
        }

        protected override void OnError(NotificacaoCommand message, Exception e)
        {
            _logger.LogError($"Falha em ler mensagem {JsonSerializer.Serialize(message)}, Erro: {e.Message}");
        }
    }
}
