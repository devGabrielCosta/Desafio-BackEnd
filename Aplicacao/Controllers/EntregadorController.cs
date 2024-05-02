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

        [HttpGet]
        public  ActionResult<ResponseModel<IEnumerable<Entregador>>> Get()
        {
            var entregadores = _service.Get();
            return Ok(new ResponseModel<IEnumerable<Entregador>>(entregadores));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<Entregador?>>> Insert(CreateEntregador request)
        {   
            var entregador = request.Mapper();

            await _service.InsertEntregadorAsync(entregador);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Entregador?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel<Entregador>(entregador));
        }

        [HttpPut("imagemCnh")]
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
