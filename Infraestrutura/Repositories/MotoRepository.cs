using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositories
{
    public class MotoRepository : BaseRepository<Moto>, IMotoRepository
    {
        public MotoRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Moto> GetByPlaca(string placa)
        {
            return _context.Set<Moto>().Where(m => EF.Functions.Like(m.Placa, $"{placa}%"));
        }
        public IQueryable<Moto> GetLocacoes()
        {
            return _context.Set<Moto>().Include("Locacoes");
        }
    }
}
