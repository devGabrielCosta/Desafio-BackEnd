using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido> , IPedidoRepository
    {
        public PedidoRepository(AppDbContext context) : base(context)
        { 
        }

        public Pedido? GetNotificados(Guid id)
        {
            return _context.Set<Pedido>().Where(p => p.Id == id).Include("Notificados").FirstOrDefault();
        }
    }
}
