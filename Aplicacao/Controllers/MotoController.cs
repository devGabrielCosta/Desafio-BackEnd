using Aplicacao.Requests;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Mappers;
using Dominio.Interfaces.Notification;
using Aplicacao.Response;
using Dominio.Entities;
using Microsoft.AspNetCore.Authorization;
using Aplicacao.Configuration;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotoController : AbstractController
    {
        private IMotoService _service { get; }
        private INotificationContext _notificationContext { get; }

        public MotoController(IMotoService service, INotificationContext notificationContext) 
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
        [HttpGet("{placa}")]
        [HttpGet("")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<IEnumerable<Moto>>> Get(string placa = "")
        {
            var motos = _service.GetByPlaca(placa);
            return Ok(new ResponseModel<IEnumerable<Moto>>(motos));
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
        public async Task<ActionResult<ResponseModel<Moto?>>> Insert(CreateMoto request)
        {
            var moto = request.Mapper();

            await _service.InsertMotoAsync(moto);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Moto?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel<Moto>(moto));
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
        public ActionResult<ResponseModel<Moto?>> Update(UpdateMoto request, Guid id)
        {
            var moto = _service.UpdatePlacaMoto(id, request.Placa);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Moto?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<Moto?>(moto));
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
        public ActionResult<ResponseModel<object?>> Delete(Guid id)
        {
            _service.DeleteMoto(id);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<object?>(null));
        }
    }
}
