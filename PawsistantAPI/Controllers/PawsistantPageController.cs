using Microsoft.AspNetCore.Mvc;

namespace PawsistantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PawsistantPageController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
