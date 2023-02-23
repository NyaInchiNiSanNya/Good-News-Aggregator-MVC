using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return RedirectToAction("Registration", "Account");
        }

        public IActionResult Start()
        {
            return View("Index");
        }


    }
}