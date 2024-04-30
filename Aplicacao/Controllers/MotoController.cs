using Aplicacao.Requests;
using Dominio.Entities;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Mappers;
using Dominio.Interfaces.Notification;
using Aplicacao.Response;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MotoController : ControllerBase
    {
        private IMotoService _service { get; }
        private INotificationContext _notificationContext { get; }

        public MotoController(IMotoService service, INotificationContext notificationContext) 
        {
            _service = service;
            _notificationContext = notificationContext;
        }

        [HttpGet("{placa}")]
        public ActionResult<ResponseModel> Get(string placa = "")
        {
            var motos = _service.GetByPlaca(placa);
            return Ok(new ResponseModel(motos));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Insert(CreateMoto request)
        {
            var moto = request.Mapper();

            await _service.InsertMotoAsync(moto);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel(null, _notificationContext.Notifications));

            return CreatedAtAction(nameof(Insert), new ResponseModel(moto));
        }
        
        [HttpPut("{id}")]
        public ActionResult<ResponseModel> Update(UpdateMoto request, Guid id)
        {
            var moto = _service.UpdatePlacaMoto(id, request.Placa);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel(null, _notificationContext.Notifications));

            return Ok(new ResponseModel(moto));
        }

        [HttpDelete("{id}")]
        public ActionResult<ResponseModel> Delete(Guid id)
        {
            _service.DeleteMoto(id);

            if (_notificationContext.HasNotifications)
                return BadRequest(new ResponseModel(null, _notificationContext.Notifications));

            return Ok();
        }
    }
}
