using Dominio.Entities;
using Infraestrutura.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Aplicacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {   
        private AdminRepository _repository { get; }

        public AdminController(AdminRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Admin> Get()
        {
            return _repository.GetAll().ToList();
        }

        [HttpPost]
        public void Insert()
        {
            _repository.Insert();
        }
    }
}
