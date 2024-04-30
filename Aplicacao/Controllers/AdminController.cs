using Aplicacao.Response;
using Dominio.Entities;
using Dominio.Interfaces.Notification;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {   
        private IAdminService _service{ get; }

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<ResponseModel> Get()
        {
            var admins = _service.Get();
            return Ok(new ResponseModel(admins));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel>> Insert()
        {
            var admin = await _service.CreateAdminAsync();

            return CreatedAtAction(nameof(Insert), new ResponseModel(admin));
        }
    }
}
