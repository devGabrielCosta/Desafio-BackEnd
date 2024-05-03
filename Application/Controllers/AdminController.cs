using Application.Responses;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : AbstractController
    {   
        private IAdminService _service{ get; }

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna administradores
        /// </summary>
        /// <response code="400">Algum dado enviado está incorreto</response>
        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<Admin>>> GetAdminList()
        {
            var admins = _service.Get();
            return Ok(new ResponseModel<IEnumerable<Admin>>(admins));
        }

        /// <summary>
        /// Insere um novo administrador
        /// </summary>
        /// <response code="201">Retorna administrador criado </response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<ResponseModel<Admin>>> InsertAdmin()
        {
            var admin = await _service.CreateAdminAsync();

            return CreatedAtAction(nameof(InsertAdmin), new ResponseModel<Admin>(admin));
        }
    }
}
