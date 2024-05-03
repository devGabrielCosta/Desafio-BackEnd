using Domain.Handlers.Commands;
using Domain.Interfaces.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.RabbitMq.Consumers
{
    public class NotifyOrderCouriersQueueConsumer : RabbitMqConsumerBase<NotifyOrderCouriersCommand>
    {
        private const string QUEUE_NAME = "notify_order_couriers";
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public NotifyOrderCouriersQueueConsumer(
            IConfiguration configuration, 
            IServiceProvider serviceProvider,
            ILogger<NotifyOrderCouriersQueueConsumer> logger
        ) : base(configuration, QUEUE_NAME, logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override void OnExecute(NotifyOrderCouriersCommand message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<ICommandHandler<NotifyOrderCouriersCommand>>();

                if (handler == null)
                    throw new NotImplementedException();

                handler.Handle(message);
            }
        }

        protected override void OnError(NotifyOrderCouriersCommand message, Exception e)
        {
            _logger.LogError($"Failure to read message {JsonSerializer.Serialize(message)}, Error: {e.Message}");
        }
    }
}
