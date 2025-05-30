using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PawsistantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("ping")]
        ///[Authorize]  // Only accessible with a valid JWT token

        public IActionResult Ping()
        {
            return Ok("Pong from server!");
        }

    }

}
