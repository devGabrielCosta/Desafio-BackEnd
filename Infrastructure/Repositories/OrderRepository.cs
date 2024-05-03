using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order> , IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        { 
        }

        public IQueryable<Order> GetNotifieds(Guid id)
        {
            return _context.Set<Order>().Where(p => p.Id == id).Include(nameof(Order.Notifieds));
        }
    }
}
