using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Infraestrutura.RabbitMq
{
    internal class RabbitMq<T>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger _logger;

        private string _queueName;
        private string _queueNameDead => _queueName+"_dead_letter";
        private string _deadExchange => "dead_letter_exchange";


        public RabbitMq(
            IConfiguration configuration, 
            string queueName,
            ILogger logger)
        {
            _logger = logger;
            _connection = CreateConnection(configuration);
            _channel = _connection.CreateModel();
            _queueName = queueName;

            Configuration();
        }

        private void Configuration()
        {
            _channel.ExchangeDeclare(_deadExchange, ExchangeType.Fanout);

            var arguments = new Dictionary<string, object>();
            arguments.Add("x-dead-letter-exchange", _deadExchange);
            _channel.QueueDeclare(queue:_queueName, exclusive: false, arguments: arguments);
            arguments.Clear();
            arguments.Add("x-message-ttl", 5000);
            arguments.Add("x-dead-letter-exchange", "amq.direct");
            _channel.QueueDeclare(queue: _queueNameDead, exclusive: false, arguments: arguments);

            _channel.QueueBind(_queueName, "amq.direct", _queueName);
            _channel.QueueBind(_queueNameDead, _deadExchange, "");
        }

        public IConnection CreateConnection(IConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(configuration.GetConnectionString("rabbitmq"))
            };

            var retryAttempts = 0;
            while (retryAttempts++ <= 3)
            {
                try
                {
                    return connectionFactory.CreateConnection();
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Conexão falhou, Tentando novamente em 3s");
                    Thread.Sleep(3000);
                }
            }

            return null;
        }

        public void Publish(T message)
        {
            _channel.BasicPublish(
                exchange: "amq.direct",
                routingKey: _queueName,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message))
            );

            _logger.LogInformation($"[{_queueName}] - Mensagem publicada.");
        }

        public void Listen(Action<T> handleMessage)
        {
            _logger.LogInformation("Iniciando consumo");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, @event) =>
            {
                _logger.LogInformation($"[{_queueName}] - Nova mensagem recebida.");

                var body = @event.Body.Span;
                var message = Encoding.UTF8.GetString(body);

                if (!string.IsNullOrEmpty(message))
                {
                    var command = JsonSerializer.Deserialize<T>(message);

                    if (command != null)
                        try
                        {
                            handleMessage.Invoke(command);
                            _channel.BasicAck(@event.DeliveryTag, false);
                        }
                        catch (Exception)
                        {
                            _channel.BasicNack(@event.DeliveryTag, false, false);
                        }

                }

            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}
