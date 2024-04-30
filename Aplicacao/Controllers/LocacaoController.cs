using Aplicacao.Mappers;
using Aplicacao.Requests;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocacaoController : Controller
    {
        private ILocacaoService _service { get; }

        public LocacaoController(ILocacaoService service)
        {
            _service = service;
        }

        [HttpPost("")]
        public Task<Locacao?> Insert(CreateLocacao request)
        {
            var locacao = request.Mapper();

            return _service.RealizarLocacao(locacao);
        }

        [HttpPost("PrevisaoDevolucao/{id}")]
        public Task<decimal> UpdateDevolucao(UpdatePrevisaoDevolucao request, Guid id)
        {
            return _service.ConsultarDevolucao(id, request.PrevisaoDevolucao);
        }
    }
}
