using LendingSystem.API.DTOs;
using LendingSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace LendingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            try
            {
                var user = await _authService.Register(dto.Username, dto.Email, dto.Password, dto.Role);
                return Ok(new { user.Id, user.Username, user.Email, user.Role });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _authService.Authenticate(dto.Username, dto.Password);
            if (token == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }

    }
}
