using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {

            switch (statusCode)
            {
                case 404:
                    ViewBag.errorMessage="Page Not Found";
                    return View("NotFound");
            }

            return View("_Layout");
        }
    }
}
