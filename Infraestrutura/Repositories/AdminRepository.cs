using Dominio.Entities;
using Infraestrutura.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositories
{
    public class AdminRepository
    {
        private AppDbContext _context;
        public AdminRepository(AppDbContext context) 
        { 
            _context = context;
        }

        public IEnumerable<Admin> GetAll()
        {
            return _context.Admins;
        }

        public void Insert()
        {
            var admin = new Admin();

            _context.Admins.Add(admin);
            _context.SaveChanges();
        }
    }
}
