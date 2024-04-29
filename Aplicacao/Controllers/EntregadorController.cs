using Aplicacao.Mappers;
using Aplicacao.Requests;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntregadorController : Controller
    {
        private IEntregadorService _service { get; }
        private IWebHostEnvironment _environment { get; }

        public EntregadorController(IEntregadorService service, IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        [HttpGet]
        public  IEnumerable<Entregador> Get()
        {
            return _service.Get();
        }

        [HttpPost]
        public async Task<Entregador> Insert(CreateEntregador request)
        {
            var entregador = request.Mapper();

            return await _service.InsertEntregador(entregador);
        }

        [HttpPut("{id}/imagemCnh")]
        public async Task<Entregador> UpdateImage(IFormFile imagem, Guid id)
        {
            if (imagem.Length <= 0)
                return null;

            if (!(imagem.ContentType == "image/png" || imagem.ContentType == "image/bmp"))
                return null;

            var url = await UploadImage(imagem);

            return await _service.UpdateCnhImagemEntregador(id, url);
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
