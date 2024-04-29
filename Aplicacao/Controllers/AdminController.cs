using Dominio.Entities;
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
        public IEnumerable<Admin> Get()
        {
            return _service.GetAdmins();
        }

        [HttpPost]
        public Admin Insert()
        {
            return _service.CreateAdmin();
        }
    }
}
