using Aplicacao.Requests;
using Dominio.Entities;

namespace Aplicacao.Mappers
{
    public static class PedidoMapper
    {
        public static Pedido Mapper(this CreatePedido request)
        {
            return new Pedido(
                request.ValorDaCorrida
            );
        }
    }
}
