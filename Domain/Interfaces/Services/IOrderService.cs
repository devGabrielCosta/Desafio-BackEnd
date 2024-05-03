using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> Get();
        Order? GetNotifieds(Guid id);
        Task<Order> InsertOrderAsync(Order order);
        void AcceptOrder(Guid id, Guid courierId);
        void FinishOrder(Guid id, Guid courierId);
    }
}
