using Domain.Handlers.Commands;
using Domain.Interfaces.Handlers;
using Domain.Interfaces.Repositories;

namespace Domain.Handlers
{
    public class NotifyOrderCouriersHandler : ICommandHandler<NotifyOrderCouriersCommand>
    {
        private ICourierRepository _courierRepository { get; }
        private IOrderRepository _orderRepository { get; }

        public NotifyOrderCouriersHandler(ICourierRepository courierRepository, IOrderRepository orderRepository)
        {
            _courierRepository = courierRepository;
            _orderRepository = orderRepository;
        }

        public void Handle(NotifyOrderCouriersCommand @event)
        {
            var availableCouriers = _courierRepository.AvailableCouriersForOrder();
            if (!availableCouriers.Any())
            {
                throw new Exception("No couriers avilable");
            }

            var order = _orderRepository.Get(@event.OrderId).FirstOrDefault();
            if(order == null)
            {
                throw new ArgumentException("OrderId not found");
            }
            
            var listNotificados = order.Notifieds.ToList();
            listNotificados.AddRange(availableCouriers.ToList());

            order.Notifieds = listNotificados;

            _orderRepository.Update(order);
        }
    }
}
