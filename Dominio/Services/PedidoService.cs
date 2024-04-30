using Dominio.Entities;
using Dominio.Handlers.Commands;
using Dominio.Interfaces.Mensageria;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;

namespace Dominio.Services
{
    public class PedidoService : IPedidoService
    {
        private IPedidoRepository _repository { get; }
        private IPublisher<NotificacaoCommand> _publisherNotificacao { get; }
        private IEntregadorService _entregadorService { get; }

        public PedidoService(
            IPedidoRepository repository, 
            IPublisher<NotificacaoCommand> publisherNotificacao,
            IEntregadorService entregadorService)
        {
            _repository = repository;
            _publisherNotificacao = publisherNotificacao;
            _entregadorService = entregadorService;
        }

        public Pedido? GetNotificados(Guid id)
        {
            return _repository.GetNotificados(id);
        }

        private Pedido? Get(Guid id)
        {
            return _repository.Get(id);
        }

        public async Task<Pedido> InsertPedido(Pedido pedido)
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
            var pedido = this.GetNotificados(id);

            if(pedido == null)
                return false;

            var entregadorNaoFoiNotificado = !pedido.Notificados.Any(e => e.Id == entregadorId);
            var pedidoNaoDisponivel = !(pedido.Situacao == Situacao.Disponivel);
            if (entregadorNaoFoiNotificado || pedidoNaoDisponivel)
                return false;

            pedido.Situacao = Situacao.Aceito;
            pedido.Entregador = entregador;
            this.UpdatePedido(pedido);

            return true;

        }

        public bool FinalizarPedido(Guid id, Guid entregadorId)
        {
            var pedido = this.Get(id);

            if (pedido.EntregadorId != entregadorId)
                return false;

            if(pedido.Situacao != Situacao.Aceito)
                return false;

            pedido.Situacao = Situacao.Entregue;
            this.UpdatePedido(pedido);

            return true;
        }
    }
}
