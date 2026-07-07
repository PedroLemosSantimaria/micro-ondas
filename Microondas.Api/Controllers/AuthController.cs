using Microsoft.AspNetCore.Mvc;
using Microondas.Core.Dtos.Requests;
using Microondas.Core.Interfaces;

namespace Microondas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var result = authService.Login(request);
            return Ok(result);
        }
    }
}