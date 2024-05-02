using Aplicacao.Configuration;
using Aplicacao.Mappers;
using Aplicacao.Requests;
using Aplicacao.Response;
using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocacaoController : AbstractController
    {
        private ILocacaoService _service { get; }
        private INotificationContext _notificationContext { get; }

        public LocacaoController(ILocacaoService service, INotificationContext notificationContext)
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        [HttpPost("")]
        [Authorize(Roles = Roles.Entregador)]
        public async Task<ActionResult<ResponseModel<Locacao?>>> Insert(CreateLocacao request)
        {
            var locacao = request.Mapper();
            locacao.EntregadorId = LoggerUserGuid();

            await _service.InsertLocacaoAsync(locacao);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Locacao?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel<Locacao>(locacao));

        }

        [HttpPost("PrevisaoDevolucao/{id}")]
        [Authorize(Roles = Roles.Entregador)]
        public ActionResult<ResponseModel<PrevisaoLocacao>> UpdateDevolucao(UpdatePrevisaoDevolucao request, Guid id)
        {
            var preco = _service.ConsultarDevolucao(id, request.PrevisaoDevolucao, LoggerUserGuid());

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<PrevisaoLocacao>(null, _notificationContext.Notifications));
            else
                return Ok(new ResponseModel<PrevisaoLocacao>(new PrevisaoLocacao(preco)));

        }
    }
}
