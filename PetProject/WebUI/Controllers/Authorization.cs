using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using Project.Abstractions;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace WebUI.Controllers
{
    [ApiController]
    public class Authorization : Microsoft.AspNetCore.Mvc.Controller
    {

         private IUserRepository _UserRepository;

         public Authorization(IUserRepository _UserRepository)
         {
            this._UserRepository= _UserRepository;
         }

         [HttpGet("api/Login")]
         public async Task<IActionResult> Login()
         {
             return View("LoginView");
         }


        [HttpGet("api/Registration")]
        public async Task<IActionResult> Registration()
         {
            return View("RegistrationView");
        }



        [HttpPost("api/Login")]
         public async Task<IActionResult> Login([FromForm] IUserRepository.UserLoginViewModel model)
         {
             if (ModelState.IsValid)
             {

                if (!await _UserRepository.Authification_method(model))
                {
                        ModelState.AddModelError("", "Неверный электронный адрес или пароль.");
                }
                else
                {
                        return View("/Views/Home/Start.cshtml"); // Validation passed
                }
             }
             return View("LoginView",model);
        }



    

        
        [HttpPost("api/Registration")]
        public async Task<IActionResult> Registration([FromForm] IUserRepository.UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            { 
                if (!await _UserRepository.Existence_Check(model))
                {
                    ModelState.AddModelError("", "Email уже существует");
                }
                else
                {
                    await _UserRepository.Add_New(model);
                    return View("LoginView"); // Validation passed
                }

            }
            
            return View("RegistrationView",model);
        }

        
    }
}
