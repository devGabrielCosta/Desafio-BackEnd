using Dominio.Entities;
using Dominio.Services;

namespace Dominio.Interfaces.Services
{
    public interface IPedidoService
    {
        Pedido? GetNotificados(Guid id);
        Task<Pedido> InsertPedido(Pedido pedido);
        bool AceitarPedido(Guid id, Guid entregadorId);
        bool FinalizarPedido(Guid id, Guid entregadorId);
    }
}
