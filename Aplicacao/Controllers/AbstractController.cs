using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aplicacao.Controllers
{
    public class AbstractController : ControllerBase
    {   
        protected Guid LoggerUserGuid()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
