using Application.Requests;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Application.Mappers;
using Domain.Interfaces.Notification;
using Application.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Application.Configuration;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotorcycleController : AbstractController
    {
        private IMotorcycleService _service { get; }
        private INotificationContext _notificationContext { get; }

        public MotorcycleController(IMotorcycleService service, INotificationContext notificationContext) 
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        /// <summary>
        /// Retorna Motos por placa
        /// </summary>
        /// <response code="200">Retorna motos</response>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpGet("{licensePlate}")]
        [HttpGet("")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<IEnumerable<Motorcycle>>> GetMotorcycles(string licensePlate = "")
        {
            var motorcycles = _service.GetByLicensePlate(licensePlate);
            return Ok(new ResponseModel<IEnumerable<Motorcycle>>(motorcycles));
        }

        /// <summary>
        /// Insere nova moto
        /// </summary>
        /// <response code="201">Retorna moto criada</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ResponseModel<Motorcycle?>>> InsertMotorcycle(CreateMotorcycle request)
        {
            var motorcycle = request.Mapper();

            await _service.InsertMotorcycleAsync(motorcycle);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Motorcycle?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(InsertMotorcycle), new ResponseModel<Motorcycle>(motorcycle));
        }

        /// <summary>
        /// Atualiza uma moto
        /// </summary>
        /// <response code="200">Retorna moto atualizada</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<Motorcycle?>> UpdateMotorcycle(UpdateMotorcycle request, Guid id)
        {
            var motorcycle = _service.UpdateMotorcycleLicensePlate(id, request.LicensePlate);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Motorcycle?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<Motorcycle?>(motorcycle));
        }


        /// <summary>
        /// Deleta uma moto
        /// </summary>
        /// <response code="200">Retorna moto atualizada</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas administradores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object?>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<object?>> DeleteMotorcycle(Guid id)
        {
            _service.DeleteMotorcycle(id);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<object?>(null));
        }
    }
}
