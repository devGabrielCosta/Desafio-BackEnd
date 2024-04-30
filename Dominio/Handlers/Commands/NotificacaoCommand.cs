using System.Windows.Input;

namespace Dominio.Handlers.Commands
{
    public class NotificacaoCommand
    {
        public Guid PedidoId { get; set; }
        public NotificacaoCommand(Guid pedidoId)
        {
            PedidoId = pedidoId;
        }
    }
}
