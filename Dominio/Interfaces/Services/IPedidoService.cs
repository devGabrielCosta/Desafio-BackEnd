using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IPedidoService
    {
        Task<Pedido> InsertPedido(Pedido pedido);
    }
}
