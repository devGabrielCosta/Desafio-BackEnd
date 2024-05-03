using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        IQueryable<Order> GetNotifieds(Guid id);
    }
}
