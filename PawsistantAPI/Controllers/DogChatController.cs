using Microsoft.AspNetCore.Mvc;

namespace PawsistantAPI.Controllers
{
    public class DogChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
