using Dominio.Interfaces.Mensageria;
using Microsoft.Extensions.Configuration;

namespace Infraestrutura.RabbitMq
{
    public abstract class RabbitMqPublisherBase<T> : IPublisher<T>
    {   
        private RabbitMq<T> _rabbitMq;

        public RabbitMqPublisherBase(IConfiguration configuration, string queueName)
        {
            _rabbitMq = new RabbitMq<T>(configuration, queueName);
        }

        public void Publish(T message)
        {
            _rabbitMq.Publish(message);
        }

    }
}
