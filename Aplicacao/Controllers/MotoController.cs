using Aplicacao.Requests;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Mappers;
using Dominio.Interfaces.Notification;
using Aplicacao.Response;
using Dominio.Entities;
using Microsoft.AspNetCore.Authorization;
using Aplicacao.Configuration;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotoController : AbstractController
    {
        private IMotoService _service { get; }
        private INotificationContext _notificationContext { get; }

        public MotoController(IMotoService service, INotificationContext notificationContext) 
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        [HttpGet("{placa}")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<IEnumerable<Moto>>> Get(string placa = "")
        {
            var motos = _service.GetByPlaca(placa);
            return Ok(new ResponseModel<IEnumerable<Moto>>(motos));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<ResponseModel<Moto?>>> Insert(CreateMoto request)
        {
            var moto = request.Mapper();

            await _service.InsertMotoAsync(moto);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Moto?>(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel<Moto>(moto));
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<Moto?>> Update(UpdateMoto request, Guid id)
        {
            var moto = _service.UpdatePlacaMoto(id, request.Placa);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<Moto?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<Moto?>(moto));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<ResponseModel<object?>> Delete(Guid id)
        {
            _service.DeleteMoto(id);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel<object?>(null, _notificationContext.Notifications));

            return Ok(new ResponseModel<object?>(null));
        }
    }
}
