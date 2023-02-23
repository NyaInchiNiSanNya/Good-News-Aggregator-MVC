using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Start()
        {
            return View("/Views/Authorization/RegistrationView.cshtml");
        }


    }
}