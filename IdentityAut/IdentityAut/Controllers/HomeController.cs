using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Start()
        {
            return View("Index");
        }


    }
}