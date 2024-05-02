using Aplicacao.Configuration;
using Aplicacao.Mappers;
using Aplicacao.Requests;
using Aplicacao.Response;
using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : AbstractController
    {   
        private IPedidoService _service { get; }
        private INotificationContext _notificationContext { get; }

        public PedidoController(IPedidoService service, INotificationContext notificationContext)
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Retorna entregadores notificados pelo pedido
        /// </summary>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpGet("{id}/Notificados")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<IEnumerable<Entregador>>> GetNoficados(Guid id)
        {
            var entregadores = _service.GetNotificados(id)?.Notificados;

            return Ok(new ResponseModel<IEnumerable<Entregador>>(entregadores));
        }

        /// <summary>
        /// Insere novo pedido
        /// </summary>
        /// <response code="201">Retorna pedido criado</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ResponseModel<Pedido?>>> Insert(CreatePedido request)
        {
            var pedido = request.Mapper();

            await _service.InsertPedidoAsync(pedido);

            return CreatedAtAction(nameof(Insert), new ResponseModel<Pedido?>(pedido));
        }

        /// <summary>
        /// Muda o status do pedido para aceito, e atribui ao entregador
        /// </summary>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPatch("{id}/Aceitar")]
        [Authorize(Roles = Roles.Entregador)]
        public ActionResult<ResponseModel<bool>> AceitarPedido(Guid id)
        {
            _service.AceitarPedido(id, LoggerUserGuid());

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<bool>(false, _notificationContext.Notifications));

            return Ok(new ResponseModel<bool>(true));
        }

        /// <summary>
        /// Muda o status do pedido para entregue
        /// </summary>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPatch("{id}/Finalizar")]
        [Authorize(Roles = Roles.Entregador)]
        public ActionResult<ResponseModel<bool>> FinalizarPedido(Guid id)
        {
            _service.FinalizarPedido(id, LoggerUserGuid());

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<bool>(false, _notificationContext.Notifications));

            return Ok(new ResponseModel<bool>(true));
        }

    }
}
