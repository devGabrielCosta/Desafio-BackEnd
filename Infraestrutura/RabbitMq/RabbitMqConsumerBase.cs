using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infraestrutura.RabbitMq
{
    public abstract class RabbitMqConsumerBase<T> : BackgroundService
    {
        private RabbitMq<T> _rabbitMq;

        public RabbitMqConsumerBase(IConfiguration configuration, string queueName)
        {
            _rabbitMq = new RabbitMq<T>(configuration, queueName);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabbitMq.Listen(this.handleMessage);
        }

        private void handleMessage(T message)
        {
            try
            {
                OnExecute(message);
            }
            catch (Exception e)
            {
                OnError(message, e);
                throw;
            }
        }

        protected abstract void OnExecute(T message);
        protected abstract void OnError(T message, Exception e);
    }
}
