using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;

namespace Dominio.Services
{
    public class PedidoService : IPedidoService
    {
        private IPedidoRepository _repository { get; }

        public PedidoService(IPedidoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Pedido> InsertPedido(Pedido pedido)
        {   
            await _repository.InsertAsync(pedido);

            //DISPARAR RABBITMQ

            return pedido;
        }
    }
}
