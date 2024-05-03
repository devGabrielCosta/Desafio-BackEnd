using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RentalRepository : BaseRepository<Rental> , IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context)
        { 
        }

        public IQueryable<Rental> Get(Guid id) 
        { 
            return base.Get(id).Include(nameof(Rental.Motorcycle));
        }
    }
}
