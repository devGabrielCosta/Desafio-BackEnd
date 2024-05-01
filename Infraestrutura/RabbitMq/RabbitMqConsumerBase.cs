using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infraestrutura.RabbitMq
{
    public abstract class RabbitMqConsumerBase<T> : BackgroundService
    {
        private RabbitMq<T> _rabbitMq;
        private ILogger _logger;

        public RabbitMqConsumerBase(
            IConfiguration configuration, 
            string queueName,
            ILogger logger)
        {
            _rabbitMq = new RabbitMq<T>(configuration, queueName, logger);
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"ExecuteAsync Iniciado");

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
