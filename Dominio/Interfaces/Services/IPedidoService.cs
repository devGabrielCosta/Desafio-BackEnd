using Dominio.Entities;

namespace Dominio.Interfaces.Services
{
    public interface IPedidoService
    {
        Pedido? GetNotificados(Guid id);
        Task<Pedido> InsertPedidoAsync(Pedido pedido);
        void AceitarPedido(Guid id, Guid entregadorId);
        void FinalizarPedido(Guid id, Guid entregadorId);
    }
}
