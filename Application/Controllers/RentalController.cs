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
    public class RentalController : AbstractController
    {
        private IRentalService _service { get; }
        private INotificationContext _notificationContext { get; }

        public RentalController(IRentalService service, INotificationContext notificationContext)
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Insere nova locação
        /// </summary>
        /// <response code="201">Retorna locação criada</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPost("")]
        [Authorize(Roles = Roles.Courier)]
        public async Task<ActionResult<ResponseModel<Rental?>>> Insert(CreateRental request)
        {
            var rental = request.Mapper();
            rental.CourierId = LoggerUserGuid();

            await _service.InsertRentalAsync(rental);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Rental?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel<Rental>(rental));

        }

        /// <summary>
        /// Atualiza data de devolução, finaliza a locação e retorna preço
        /// </summary>
        /// <response code="200">Retorna preço</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPatch("{id}/ReportReturn")]
        [Authorize(Roles = Roles.Courier)]
        public ActionResult<ResponseModel<ReturnRentalPrice>> ReportReturn(UpdateReturnDate request, Guid id)
        {
            var price = _service.ReportReturn(id, request.ReturnDate, LoggerUserGuid());

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<ReturnRentalPrice>(null, _notificationContext.Notifications));
            else
                return Ok(new ResponseModel<ReturnRentalPrice>(new ReturnRentalPrice(price)));

        }
    }
}
