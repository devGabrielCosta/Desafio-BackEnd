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
    public class LocacaoController : ControllerBase
    {
        private ILocacaoService _service { get; }
        private INotificationContext _notificationContext { get; }

        public LocacaoController(ILocacaoService service, INotificationContext notificationContext)
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        [HttpPost("")]
        public async Task<ActionResult<ResponseModel<Locacao?>>> Insert(CreateLocacao request)
        {
            var locacao = request.Mapper();

            await _service.InsertLocacaoAsync(locacao);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Locacao?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel<Locacao>(locacao));

        }

        [HttpPost("PrevisaoDevolucao/{id}")]
        public ActionResult<ResponseModel<object?>> UpdateDevolucao(UpdatePrevisaoDevolucao request, Guid id)
        {
            var preco = _service.ConsultarDevolucao(id, request.PrevisaoDevolucao);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<object?>(preco, _notificationContext.Notifications));
            else
                return Ok(new ResponseModel<object?>(new  { Preco = preco } ));

        }
    }
}
