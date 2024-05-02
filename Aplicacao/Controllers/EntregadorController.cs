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
    public class EntregadorController : AbstractController
    {
        private IEntregadorService _service { get; }
        private IWebHostEnvironment _environment { get; }
        private INotificationContext _notificationContext { get; }

        public EntregadorController(
            IEntregadorService service, 
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
        public  ActionResult<ResponseModel<IEnumerable<Entregador>>> Get()
        {
            var entregadores = _service.Get();
            return Ok(new ResponseModel<IEnumerable<Entregador>>(entregadores));
        }

        /// <summary>
        /// Insere novo entregador
        /// </summary>
        /// <response code="201">Retorna entregador criado</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel<Entregador?>>> Insert(CreateEntregador request)
        {   
            var entregador = request.Mapper();

            await _service.InsertEntregadorAsync(entregador);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Entregador?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel<Entregador>(entregador));
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
        [HttpPatch("imagemCnh")]
        [Authorize(Roles = Roles.Entregador)]
        public async Task<ActionResult<ResponseModel<Entregador?>>> UpdateImage(IFormFile imagem)
        {
            if (imagem.Length <= 0)
                _notificationContext.AddNotification("Necessário enviar uma imagem");

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Entregador?>(null, _notificationContext.Notifications));

            var uploadImage = new Dominio.Utilities.File(
                imagem.OpenReadStream(), 
                imagem.FileName.Split(".")[0], 
                imagem.ContentType.Split("/")[1]
            );

            var entregador = await _service.UpdateCnhImagemEntregador(LoggerUserGuid(), uploadImage);
            
            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Entregador?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<Entregador>(entregador));
        }

    }
}
