using LendingSystem.API.DTOs.User;
using LendingSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Register(UserRegisterDto registerDto)
    {
        try
        {
            var user = await _authService.Register(registerDto.Username, registerDto.Email, registerDto.Password, registerDto.Role);
            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto loginDto)
    {
        var token = await _authService.Authenticate(loginDto.Username, loginDto.Password);
        if (token == null)
            return Unauthorized("Invalid username or password");

        return Ok(new { token });
    }
}
