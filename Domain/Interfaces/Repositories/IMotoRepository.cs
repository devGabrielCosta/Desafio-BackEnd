using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IMotoRepository : IBaseRepository<Motorcycle>
    {
        IQueryable<Motorcycle> GetByLicensePlate(string licensePlate);
        IQueryable<Motorcycle> GetRentals();
    }
}
