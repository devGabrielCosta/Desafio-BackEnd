using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;

namespace Infraestrutura.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido> , IPedidoRepository
    {
        public PedidoRepository(AppDbContext context) : base(context)
        { 
        }
    }
}
