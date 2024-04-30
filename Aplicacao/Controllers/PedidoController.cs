using Aplicacao.Mappers;
using Aplicacao.Requests;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {   
        private IPedidoService _service{ get; }

        public PedidoController(IPedidoService service)
        {
            _service = service;
        }

        [HttpGet("{id}/Notificados")]
        public IEnumerable<Entregador>? GetNoficados(Guid id)
        {
            return _service.GetNotificados(id)?.Notificados;
        }

        [HttpPost]
        public async Task<Pedido> Insert(CreatePedido request)
        {
            var pedido = request.Mapper();

            return await _service.InsertPedido(pedido);
        }

        [HttpPost("{id}/Aceitar")]
        public bool AceitarPedido(Guid id, Guid entregadorId)
        {
            return _service.AceitarPedido(id, entregadorId);
        }

        [HttpPost("{id}/Finalizar")]
        public bool FinalizarPedido(Guid id, Guid entregadorId)
        {
            return _service.FinalizarPedido(id, entregadorId);
        }

    }
}
