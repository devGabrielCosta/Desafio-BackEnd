using Application.Configuration;
using Application.Mappers;
using Application.Requests;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Services;
using Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourierController : AbstractController
    {
        private ICourierService _service { get; }
        private IWebHostEnvironment _environment { get; }
        private INotificationContext _notificationContext { get; }

        public CourierController(
            ICourierService service, 
            IWebHostEnvironment environment, 
            INotificationContext notificationContext
        )
        {
            _service = service;
            _environment = environment;
            _notificationContext = notificationContext;
        }


        /// <summary>
        /// Retorna entregadores
        /// </summary>
        [HttpGet]
        public  ActionResult<ResponseModel<IEnumerable<Courier>>> GetCouriers()
        {
            var couriers = _service.Get();
            return Ok(new ResponseModel<IEnumerable<Courier>>(couriers));
        }

        /// <summary>
        /// Insere novo entregador
        /// </summary>
        /// <response code="201">Retorna entregador criado</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel<Courier?>>> InsertCourier(CreateCourier request)
        {   
            var courier = request.Mapper();

            await _service.InsertCourierAsync(courier);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Courier?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(InsertCourier), new ResponseModel<Courier>(courier));
        }

        /// <summary>
        /// Atualiza imagem da CNH do entregador
        /// </summary>
        /// <response code="200">Retorna entregador com imagem atualizada</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPatch("CnhImage")]
        [Authorize(Roles = Roles.Courier)]
        public async Task<ActionResult<ResponseModel<Courier?>>> UpdateCourierImage(IFormFile image)
        {
            if (image.Length <= 0)
                _notificationContext.AddNotification(ErrorNotifications.NECESSARY_SEND_IMAGE);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Courier?>(null, _notificationContext.Notifications));

            var uploadImage = new Domain.Utilities.File(
                image.OpenReadStream(), 
                image.FileName.Split(".")[0], 
                image.ContentType.Split("/")[1]
            );

            var courier = await _service.UpdateCourierCnhImage(LoggerUserGuid(), uploadImage);
            
            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Courier?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<Courier>(courier));
        }

    }
}
