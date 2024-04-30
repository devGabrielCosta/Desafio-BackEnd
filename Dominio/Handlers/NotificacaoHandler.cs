using Dominio.Entities;
using Dominio.Handlers.Commands;
using Dominio.Interfaces.Handlers;
using Dominio.Interfaces.Repositories;

namespace Dominio.Handlers
{
    public class NotificacaoHandler : ICommandHandler<NotificacaoCommand>
    {
        private IEntregadorRepository _entregadorRepository { get; }
        private IPedidoRepository _pedidoRepository { get; }

        public NotificacaoHandler(IEntregadorRepository entregadorRepository, IPedidoRepository pedidoRepository)
        {
            _entregadorRepository = entregadorRepository;
            _pedidoRepository = pedidoRepository;
        }

        public void Handle(NotificacaoCommand @event)
        {
            var entregadoresDisponiveis = _entregadorRepository.EntregadoresAptosPedido();
            if (!entregadoresDisponiveis.Any())
            {
                Console.WriteLine("Nenhum Entregador Disponivel");
                return;
            }

            var pedido = _pedidoRepository.Get(@event.PedidoId).FirstOrDefault();
            if(pedido == null)
            {
                throw new ArgumentException("PedidoId não encontrado");
            }
            
            var listNotificados = pedido.Notificados.ToList();
            listNotificados.AddRange(entregadoresDisponiveis.ToList());

            pedido.Notificados = listNotificados;

            _pedidoRepository.Update(pedido);
        }
    }
}
