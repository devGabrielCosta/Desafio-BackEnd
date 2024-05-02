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

        /// <summary>
        /// Insere nova locação
        /// </summary>
        /// <response code="201">Retorna locação criada</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Atualiza data da previsão de devolução e retorna preço
        /// </summary>
        /// <response code="200">Retorna preço</response>
        /// <response code="400">Algum dado enviado está incorreto</response>
        /// <response code="403">Apenas entregadores podem utilizar a rota</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status403Forbidden)]
        [HttpPatch("{id}/PrevisaoDevolucao")]
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
