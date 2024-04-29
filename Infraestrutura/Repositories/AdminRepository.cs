using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositories
{
    public class AdminRepository : BaseRepository<Admin> , IAdminRepository
    {
        public AdminRepository(AppDbContext context) : base(context)
        { 
        }
    }
}
