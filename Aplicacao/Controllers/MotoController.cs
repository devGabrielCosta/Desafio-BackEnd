using Aplicacao.Requests;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Mappers;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotoController : Controller
    {
        public IMotoService _service { get; }
        public MotoController(IMotoService service) 
        {
            _service = service;
        }

        [HttpGet("{placa}")]
        public IEnumerable<Moto> Get(string placa = "")
        {
            return _service.GetByPlaca(placa);
        }

        [HttpPost]
        public Task<Moto> Insert(CreateMoto request)
        {
            var moto = request.Mapper();
            return _service.InsertMoto(moto);
        }
        
        [HttpPut("{id}")]
        public Task<Moto> Update(UpdateMoto request, Guid id)
        {
            return _service.UpdatePlacaMoto(id, request.Placa);
        }

        [HttpDelete("{id}")]
        public Task Delete(Guid id)
        {
            return _service.DeleteMoto(id);
        }
    }
}
