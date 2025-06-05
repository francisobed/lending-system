using Microsoft.AspNetCore.Mvc;

namespace LendingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("LendingSystem API is running.");
    }
}
