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
        private readonly IModel _model;
        private string _queueName;
        private readonly ILogger _logger;

        public RabbitMq(
            IConfiguration configuration, 
            string queueName,
            ILogger logger)
        {
            _connection = CreateConnection(configuration);
            _model = _connection.CreateModel();
            _model.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _queueName = queueName;
            _logger = logger;
        }

        public IConnection CreateConnection(IConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(configuration.GetConnectionString("rabbitmq"))
            };

            return connectionFactory.CreateConnection();
        }

        public void Publish(T message)
        {
            _model.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message))
            );

            _logger.LogInformation($"[{_queueName}] - Mensagem publicada.");
        }

        public void Listen(Action<T> handleMessage)
        {
            _logger.LogInformation("Iniciando consumo");

            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, @event) =>
            {
                _logger.LogInformation($"[{_queueName}] - Nova mensagem recebida.");

                var body = @event.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                if (!string.IsNullOrEmpty(message))
                {
                    var command = JsonSerializer.Deserialize<T>(message);

                    if (command != null)
                        try
                        {
                            handleMessage.Invoke(command);
                            _model.BasicAck(@event.DeliveryTag, true);
                        }
                        catch (Exception)
                        {

                        }

                }

            };

            _model.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}
