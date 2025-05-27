using Microsoft.AspNetCore.Mvc;

namespace PawsistantAPI.Controllers
{
    public class PawsistantPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
