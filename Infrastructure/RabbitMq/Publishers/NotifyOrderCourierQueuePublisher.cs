using Domain.Handlers.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RabbitMq.Publishers
{
    public class NotifyOrderCourierQueuePublisher : RabbitMqPublisherBase<NotifyOrderCouriersCommand>
    {
        private const string QUEUE_NAME = "notify_order_couriers";
        private IServiceProvider _serviceProvider;

        public NotifyOrderCourierQueuePublisher(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            ILogger<NotifyOrderCourierQueuePublisher> logger
            ) : base(configuration, QUEUE_NAME, logger)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
