using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
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

        public RabbitMq(IConfiguration configuration, string queueName)
        {
            _connection = CreateConnection(configuration);
            _model = _connection.CreateModel();
            _model.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _queueName = queueName;
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
        }

        public void Listen(Action<T> handleMessage)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, @event) =>
            {
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
                        catch(Exception e) 
                        {
                            throw;
                        }

                }

            };

            _model.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }
    }
}
