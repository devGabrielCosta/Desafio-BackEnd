using Dominio.Entities;
using Dominio.Handlers.Commands;
using Dominio.Interfaces.Mensageria;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;

namespace Dominio.Services
{
    public class PedidoService : IPedidoService
    {
        private IPedidoRepository _repository { get; }
        private IPublisher<NotificacaoCommand> _publisherNotificacao { get; }
        private IEntregadorService _entregadorService { get; }
        private INotificationContext _notificationContext { get; }

        public PedidoService(
            IPedidoRepository repository, 
            IPublisher<NotificacaoCommand> publisherNotificacao,
            IEntregadorService entregadorService,
            INotificationContext notificationContext)
        {
            _repository = repository;
            _publisherNotificacao = publisherNotificacao;
            _entregadorService = entregadorService;
            _notificationContext = notificationContext;
        }

        public Pedido? GetNotificados(Guid id)
        {
            return _repository.GetNotificados(id).FirstOrDefault();
        }

        private Pedido? Get(Guid id)
        {
            return _repository.Get(id).FirstOrDefault();
        }

        public async Task<Pedido> InsertPedidoAsync(Pedido pedido)
        {
            await _repository.InsertAsync(pedido);

            var command = new NotificacaoCommand(pedido.Id);
            _publisherNotificacao.Publish(command);

            return pedido;
        }

        private void UpdatePedido(Pedido pedido)
        {
            _repository.Update(pedido);
        }

        public bool AceitarPedido(Guid id, Guid entregadorId)
        {
            var entregador = _entregadorService.Get(entregadorId);
            if(entregador == null)
            {
                _notificationContext.AddNotification("Entregador não encontrado");
                return false;
            }

            var pedido = this.GetNotificados(id);
            if(pedido == null)
            {
                _notificationContext.AddNotification("Pedido não encontrado");
                return false;
            }

            var entregadorNaoFoiNotificado = !pedido.Notificados.Any(e => e.Id == entregadorId);
            var pedidoNaoDisponivel = !(pedido.Situacao == Situacao.Disponivel);
            if (entregadorNaoFoiNotificado)
            {
                _notificationContext.AddNotification("Entregador não recebeu notificação");
                return false;
            }
            if(pedidoNaoDisponivel)
            {
                _notificationContext.AddNotification("Pedido não está com status Disponivel");
                return false;
            }

            pedido.Situacao = Situacao.Aceito;
            pedido.Entregador = entregador;
            this.UpdatePedido(pedido);

            return true;

        }

        public bool FinalizarPedido(Guid id, Guid entregadorId)
        {
            var pedido = this.Get(id);

            if (pedido.EntregadorId != entregadorId)
            {
                _notificationContext.AddNotification("Pedido não pertence ao entregador");
                return false;
            }

            if (pedido.Situacao != Situacao.Aceito)
            {
                _notificationContext.AddNotification("Pedido ainda não foi aceito");
                return false;
            }

            pedido.Situacao = Situacao.Entregue;
            this.UpdatePedido(pedido);

            return true;
        }
    }
}
