using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MotorcycleRepository : BaseRepository<Motorcycle>, IMotoRepository
    {
        public MotorcycleRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Motorcycle> GetByLicensePlate(string licensePlate)
        {
            return _context.Set<Motorcycle>().Where(m => EF.Functions.Like(m.LicensePlate, $"{licensePlate}%"));
        }
        public IQueryable<Motorcycle> GetRentals()
        {
            return _context.Set<Motorcycle>().Include(nameof(Motorcycle.Rentals));
        }
    }
}
