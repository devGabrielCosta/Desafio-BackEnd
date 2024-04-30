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

        [HttpPost]
        public async Task<Pedido> Insert(CreatePedido request)
        {
            var pedido = request.Mapper();

            return await _service.InsertPedido(pedido);
        }
    }
}
