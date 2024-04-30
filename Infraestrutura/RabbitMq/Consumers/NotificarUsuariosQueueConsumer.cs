using Dominio.Handlers.Commands;
using Dominio.Interfaces.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Infraestrutura.RabbitMq.Consumers
{
    public class NotificarUsuariosQueueConsumer : RabbitMqConsumerBase<NotificacaoCommand>
    {
        private const string QUEUE_NAME = "Notificar_Entregadores_Ativos";
        private IServiceProvider _serviceProvider;

        public NotificarUsuariosQueueConsumer(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, QUEUE_NAME)
        {
            _serviceProvider = serviceProvider;
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
            Console.WriteLine($"Falha em ler mensagem{JsonSerializer.Serialize(message)}, Erro: {e.Message}");
        }
    }
}
