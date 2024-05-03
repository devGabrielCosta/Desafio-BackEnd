using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CourierRepository : BaseRepository<Courier> , ICourierRepository
    {
        public CourierRepository(AppDbContext context) : base(context)
        { 
        }

        public IQueryable<Courier> GetRentals()
        {
            return _context.Set<Courier>().Include(nameof(Courier.Rentals));
        }

        public IQueryable<Courier> AvailableCouriersForOrder()
        {
            var courier = _context.Set<Courier>().Include(nameof(Courier.Rentals));
            var courierWithRental = courier.Where(e => e.Rentals.Any(l => l.Active)).Include(nameof(Courier.Orders));
            var courierWithoutOrder = courierWithRental.Where(e => !e.Orders.Any(p => p.Status == Status.Available));
            return courierWithoutOrder;
        }
    }
}
