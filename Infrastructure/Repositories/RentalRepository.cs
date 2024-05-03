using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class RentalRepository : BaseRepository<Rental> , IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context)
        { 
        }
    }
}
