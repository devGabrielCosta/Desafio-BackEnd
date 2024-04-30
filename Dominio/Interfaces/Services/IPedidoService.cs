using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IPedidoService
    {
        Pedido? GetNotificados(Guid id);
        Task<Pedido> InsertPedidoAsync(Pedido pedido);
        bool AceitarPedido(Guid id, Guid entregadorId);
        bool FinalizarPedido(Guid id, Guid entregadorId);
    }
}
