using Aplicacao.Mappers;
using Aplicacao.Requests;
using Aplicacao.Response;
using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {   
        private IPedidoService _service { get; }
        private INotificationContext _notificationContext { get; }

        public PedidoController(IPedidoService service, INotificationContext notificationContext)
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        [HttpGet("{id}/Notificados")]
        public ActionResult<ResponseModel<IEnumerable<Entregador>>> GetNoficados(Guid id)
        {
            var entregadores = _service.GetNotificados(id)?.Notificados;

            return Ok(new ResponseModel<IEnumerable<Entregador>>(entregadores));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<Pedido?>>> Insert(CreatePedido request)
        {
            var pedido = request.Mapper();

            await _service.InsertPedidoAsync(pedido);

            return CreatedAtAction(nameof(Insert), new ResponseModel<Pedido?>(pedido));
        }

        [HttpPost("{id}/Aceitar")]
        public ActionResult<ResponseModel<object?>> AceitarPedido(Guid id, Guid entregadorId)
        {
            _service.AceitarPedido(id, entregadorId);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<object?>(null));
        }

        [HttpPost("{id}/Finalizar")]
        public ActionResult<ResponseModel<object?>> FinalizarPedido(Guid id, Guid entregadorId)
        {
            _service.FinalizarPedido(id, entregadorId);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<object?>(null));
        }

    }
}
