using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;

namespace Infraestrutura.Repositories
{
    public class AdminRepository : BaseRepository<Admin> , IAdminRepository
    {
        public AdminRepository(AppDbContext context) : base(context)
        { 
        }
    }
}
