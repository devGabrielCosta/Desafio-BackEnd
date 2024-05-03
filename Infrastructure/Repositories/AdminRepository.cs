using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class AdminRepository : BaseRepository<Admin> , IAdminRepository
    {
        public AdminRepository(AppDbContext context) : base(context)
        { 
        }
    }
}
