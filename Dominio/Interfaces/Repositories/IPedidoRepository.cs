using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        IQueryable<Pedido> GetNotificados(Guid id);
    }
}
