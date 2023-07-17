using ApisJwt.Services.Auth;
using ApisJwt.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApisJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.Register(model);

            if (!result.Status)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
