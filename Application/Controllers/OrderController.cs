using Application.Configuration;
using Application.Mappers;
using Application.Requests;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : AbstractController
    {   
        private IOrderService _service { get; }
        private INotificationContext _notificationContext { get; }

        public OrderController(IOrderService service, INotificationContext notificationContext)
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Retorna pedidos
        /// </summary>
        /// <response code="200">Retorna lista de pedidos</response>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<IEnumerable<Order>>> GetOrders()
        {
            var orders = _service.Get();

            return Ok(new ResponseModel<IEnumerable<Order>>(orders));
        }

        /// <summary>
        /// Retorna entregadores notificados pelo pedido
        /// </summary>
        /// <response code="200">Retorna lista de entregadores</response>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpGet("{id}/Notifieds")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<IEnumerable<Courier>>> GetNofieds(Guid id)
        {
            var notifieds = _service.GetNotifieds(id)?.Notifieds;

            return Ok(new ResponseModel<IEnumerable<Courier>>(notifieds));
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
        public async Task<ActionResult<ResponseModel<Order?>>> InsertOrder(CreateOrder request)
        {
            var order = request.Mapper();

            await _service.InsertOrderAsync(order);

            return CreatedAtAction(nameof(InsertOrder), new ResponseModel<Order?>(order));
        }

        /// <summary>
        /// Muda o status do pedido para aceito, e atribui ao entregador
        /// </summary>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<bool>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPatch("{id}/Accept")]
        [Authorize(Roles = Roles.Courier)]
        public ActionResult<ResponseModel<bool>> AcceptOrder(Guid id)
        {
            _service.AcceptOrder(id, LoggerUserGuid());

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
        [HttpPatch("{id}/Finish")]
        [Authorize(Roles = Roles.Courier)]
        public ActionResult<ResponseModel<bool>> FinishOrder(Guid id)
        {
            _service.FinishOrder(id, LoggerUserGuid());

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<bool>(false, _notificationContext.Notifications));

            return Ok(new ResponseModel<bool>(true));
        }

    }
}
