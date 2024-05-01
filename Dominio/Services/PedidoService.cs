using Dominio.Entities;
using Dominio.Handlers.Commands;
using Dominio.Interfaces.Mensageria;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Dominio.Services
{
    public class PedidoService : IPedidoService
    {
        private IPedidoRepository _repository { get; }
        private IPublisher<NotificacaoCommand> _publisherNotificacao { get; }
        private IEntregadorService _entregadorService { get; }
        private INotificationContext _notificationContext { get; }
        private ILogger _logger { get; }

        public PedidoService(
            IPedidoRepository repository, 
            IPublisher<NotificacaoCommand> publisherNotificacao,
            IEntregadorService entregadorService,
            INotificationContext notificationContext,
            ILogger<PedidoService> logger)
        {
            _repository = repository;
            _publisherNotificacao = publisherNotificacao;
            _entregadorService = entregadorService;
            _notificationContext = notificationContext;
            _logger = logger;
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

        public void AceitarPedido(Guid id, Guid entregadorId)
        {
            var entregador = _entregadorService.Get(entregadorId);
            if(entregador == null)
            {
                _notificationContext.AddNotification("Entregador não encontrado");
                return;
            }

            var pedido = this.GetNotificados(id);
            if(pedido == null)
            {
                _notificationContext.AddNotification("Pedido não encontrado");
                return;
            }

            var entregadorNaoFoiNotificado = !pedido.Notificados.Any(e => e.Id == entregadorId);
            var pedidoNaoDisponivel = !(pedido.Situacao == Situacao.Disponivel);
            if (entregadorNaoFoiNotificado)
            {
                _notificationContext.AddNotification("Entregador não recebeu notificação");
                return;
            }
            if(pedidoNaoDisponivel)
            {
                _notificationContext.AddNotification("Pedido não está com status Disponivel");
                return;
            }

            pedido.Situacao = Situacao.Aceito;
            pedido.Entregador = entregador;
            this.UpdatePedido(pedido);
        }

        public void FinalizarPedido(Guid id, Guid entregadorId)
        {
            var pedido = this.Get(id);

            if (pedido.EntregadorId != entregadorId)
            {
                _notificationContext.AddNotification("Pedido não pertence ao entregador");
                return;
            }

            if (pedido.Situacao != Situacao.Aceito)
            {
                _notificationContext.AddNotification("Pedido ainda não foi aceito");
                return;
            }

            pedido.Situacao = Situacao.Entregue;
            this.UpdatePedido(pedido);

            _logger.LogInformation($"PedidoId. {pedido.Id}. Finalizado.");
        }
    }
}
