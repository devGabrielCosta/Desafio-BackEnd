﻿using Domain.Interfaces.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RabbitMq
{
    public abstract class RabbitMqPublisherBase<T> : IPublisher<T>
    {   
        private RabbitMq<T> _rabbitMq;

        public RabbitMqPublisherBase(IConfiguration configuration, string queueName, ILogger logger)
        {
            _rabbitMq = new RabbitMq<T>(configuration, queueName, logger);
        }

        public void Publish(T message)
        {
            _rabbitMq.Publish(message);
        }

    }
}
