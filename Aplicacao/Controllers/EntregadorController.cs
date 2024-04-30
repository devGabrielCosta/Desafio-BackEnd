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
    public class EntregadorController : ControllerBase
    {
        private IEntregadorService _service { get; }
        private IWebHostEnvironment _environment { get; }
        private INotificationContext _notificationContext { get; }

        public EntregadorController(IEntregadorService service, IWebHostEnvironment environment, INotificationContext notificationContext)
        {
            _service = service;
            _environment = environment;
            _notificationContext = notificationContext;
        }

        [HttpGet]
        public  ActionResult<ResponseModel> Get()
        {
            var entregadores = _service.Get();
            return Ok(new ResponseModel(entregadores));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Insert(CreateEntregador request)
        {   
            var entregador = request.Mapper();

            await _service.InsertEntregadorAsync(entregador);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel(entregador));
        }

        [HttpPut("{id}/imagemCnh")]
        public async Task<ActionResult<ResponseModel>> UpdateImage(IFormFile imagem, Guid id)
        {
            if (imagem.Length <= 0)
                _notificationContext.AddNotification("Necessário enviar uma imagem");

            if (!(imagem.ContentType == "image/png" || imagem.ContentType == "image/bmp"))
                _notificationContext.AddNotification("A imagem deve ser do tipo png ou bmp");

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel(null, _notificationContext.Notifications));

            var url = await UploadImage(imagem);
            var entregador = _service.UpdateCnhImagemEntregador(id, url);

            return Ok(new ResponseModel(entregador));
        }

        private async Task<string> UploadImage(IFormFile imagem)
        {         
            try
            {   
                var Path = _environment.ContentRootPath + "\\Imagens\\";

                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);

                using (FileStream filestream = System.IO.File.Create(Path + imagem.FileName))
                {
                    await imagem.CopyToAsync(filestream);
                    filestream.Flush();
                    return Path + imagem.FileName;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
