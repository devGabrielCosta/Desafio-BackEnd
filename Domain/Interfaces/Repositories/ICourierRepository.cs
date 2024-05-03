using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface ICourierRepository : IBaseRepository<Courier>
    {
        IQueryable<Courier> GetRentals();
        IQueryable<Courier> AvailableCouriersForOrder();
    }
}
