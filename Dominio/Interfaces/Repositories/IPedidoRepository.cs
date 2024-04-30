using Dominio.Entities;

namespace Dominio.Interfaces.Repositories
{
    public interface IPedidoRepository : IBaseRepository<Pedido>
    {
        Pedido? GetNotificados(Guid id);
    }
}
