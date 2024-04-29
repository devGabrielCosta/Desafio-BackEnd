using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;

namespace Infraestrutura.Repositories
{
    public class EntregadorRepository : BaseRepository<Entregador> , IEntregadorRepository
    {
        public EntregadorRepository(AppDbContext context) : base(context)
        { 
        }
    }
}
