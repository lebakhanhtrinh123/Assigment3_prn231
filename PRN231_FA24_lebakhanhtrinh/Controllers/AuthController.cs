using Microsoft.AspNetCore.Mvc;
using Repository.Infrastructure;
using Repository.Request;
using Service.Interface;

namespace PRN231_FA24_lebakhanhtrinh_BE.Controllers
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
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await authService.Login(loginRequest);
                return Ok(result);
            }
            catch (ErrorException ex) when (ex.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await authService.Register(registerRequest);
                return Ok(new { message = result });
            }
            catch (ErrorException ex) when (ex.StatusCode == StatusCodes.Status409Conflict)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
