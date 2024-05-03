using Domain.Entities;
using Domain.Handlers.Commands;
using Domain.Interfaces.Messaging;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Utilities;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _repository { get; }
        private IPublisher<NotifyOrderCouriersCommand> _publisherNotification { get; }
        private ICourierService _courierService { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }

        public OrderService(
            IOrderRepository repository, 
            IPublisher<NotifyOrderCouriersCommand> publisherNotification,
            ICourierService courierService,
            INotificationContext notificationContext,
            ILogger<OrderService> logger)
        {
            _repository = repository;
            _publisherNotification = publisherNotification;
            _courierService = courierService;
            _notificationContext = notificationContext;
            _logger = logger;
        }

        public Order? GetNotifieds(Guid id)
        {
            return _repository.GetNotifieds(id).FirstOrDefault();
        }

        private Order? Get(Guid id)
        {
            return _repository.Get(id).FirstOrDefault();
        }

        public async Task<Order> InsertOrderAsync(Order order)
        {
            await _repository.InsertAsync(order);

            var command = new NotifyOrderCouriersCommand(order.Id);
            _publisherNotification.Publish(command);

            return order;
        }

        private void UpdateOrder(Order order)
        {
            _repository.Update(order);
        }

        public void AcceptOrder(Guid id, Guid courierId)
        {
            var courier = _courierService.Get(courierId);
            if(courier == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.COURIER_NOT_FOUND);
                return;
            }

            var order = this.GetNotifieds(id);
            if(order == null)
            {
                _notificationContext.AddNotification(ErrorNotifications.ORDER_NOT_FOUND);
                return;
            }

            var courierNotNotified = !order.Notifieds.Any(e => e.Id == courierId);
            var orderUnavailable = !(order.Status == Status.Available);
            if (courierNotNotified)
            {
                _notificationContext.AddNotification(ErrorNotifications.COURIER_NOT_NOTIFIED);
                return;
            }
            if(orderUnavailable)
            {
                _notificationContext.AddNotification(ErrorNotifications.ORDER_UNAVAILABLE);
                return;
            }

            order.Status = Status.Accepted;
            order.Courier = courier;
            this.UpdateOrder(order);
        }

        public void FinishOrder(Guid id, Guid courierId)
        {
            var order = this.Get(id);

            if (order.CourierId != courierId)
            {
                _notificationContext.AddNotification(ErrorNotifications.ORDER_COURIER_NOT_AUTHORIZED);
                return;
            }

            if (order.Status != Status.Accepted)
            {
                _notificationContext.AddNotification(ErrorNotifications.ORDER_NOT_ACCEPTED_OR_DELIVERED);
                return;
            }

            order.Status = Status.Delivered;
            this.UpdateOrder(order);

            _logger.LogInformation($"OrderId. {order.Id}. Delivered.");
        }
    }
}
