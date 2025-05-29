using Library.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using PawsistantAPI.Services.Interfaces;

namespace PawsistantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PawsistantPageController : ControllerBase
    {
        public readonly IPawsistantService _pawsistantService;

        public PawsistantPageController(IPawsistantService pawsistantService)
        {
            _pawsistantService = pawsistantService;
        }


        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatMessage message)
        {
            var aiResponse = await _pawsistantService.GetResponseAsync(message);
            return Ok(aiResponse);
        }
    }
}
