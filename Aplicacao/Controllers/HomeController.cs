using Aplicacao.Configuration;
using Aplicacao.Response;
using Dominio.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Aplicacao.Controllers
{
    [ApiController]
    public class HomeController : AbstractController
    {
        private IConfiguration _configuration { get; }
        private IAdminService _adminService { get; }
        private IEntregadorService _entregadorService { get; }

        public HomeController(IConfiguration configuration, IAdminService adminService, IEntregadorService entregadorService)
        {
            _configuration = configuration;
            _adminService = adminService;
            _entregadorService = entregadorService;
        }

        /// <summary>
        /// Simula o login apenas por id
        /// </summary>
        /// <response code="400">Id não encontrado</response>
        [ProducesResponseType(typeof(AuthenticationToken), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel<object>), StatusCodes.Status400BadRequest)]
        [HttpPost("login/{id}")]
        public ActionResult<AuthenticationToken> Login(Guid id)
        {
            var admin = _adminService.Get().FirstOrDefault(x => x.Id == id);
            if (admin != null)
                return Ok(GenerateToken(id, Roles.Admin));

            var entregador = _entregadorService.Get(id);
            if (entregador != null)
                return Ok(GenerateToken(id, Roles.Entregador));

            return NotFound();
        }

        private AuthenticationToken GenerateToken(Guid id, string role)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Role, role)
                })
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationToken(tokenHandler.WriteToken(token));
        }
    }
}
