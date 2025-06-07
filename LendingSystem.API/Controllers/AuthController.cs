using LendingSystem.Infrastructure.Services;
using LendingSystem.API.DTOs;
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
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var user = await _authService.RegisterAsync(dto.Email, dto.Password);
                return Ok(new { user.Id, user.Email });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _authService.AuthenticateAsync(dto.Email, dto.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            // For brevity, skipping JWT token generation here
            return Ok(new { user.Id, user.Email, user.Role });
        }
    }
}
