using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Repositores;
using Services.Account;
using UserConfigRepositores;

namespace CustomIdentityApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly IdentityRepository _IdentityRepository;
        private readonly GetSetUserConfigRepositore _userConfigRepositore;

        public AccountController
            (IdentityRepository IdentityRepository,
                GetSetUserConfigRepositore userConfigRepositore)
        {
            if (IdentityRepository is null)
            {
                throw new NullReferenceException();

            }
            _IdentityRepository = IdentityRepository;
           
            
            if (userConfigRepositore is null)
            {
                throw new NullReferenceException();

            }

            _userConfigRepositore = userConfigRepositore;
        }


        [HttpGet]
        public IActionResult Registration()
        {
            return View("Registration");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Registration
            (UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _IdentityRepository.Registration
                         (model.Email, model.Password))
                {

                    await SetDefoultUserConfig(model);

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    
                    ModelState.AddModelError(""
                        , "Email уже существует");
                }
                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                
                if (await _IdentityRepository.Login
                        (model.Email, model.Password))
                {

                    await TakeUserConfigPutInCookie(model);
                    return RedirectToAction("Start", "Home");
                }
                else
                {
                    ModelState.AddModelError(""
                        , "Неправильный логин и (или) пароль");
                }

            }
            return View(model);
        }

        [NonAction]
        private async Task SetDefoultUserConfig(UserRegistrationViewModel model)
        {
            await _userConfigRepositore.SetUserConfig(new Dictionary<string, string>()
            {
                { "Name", model.Name },
                { "Email", model.Email },
                { "Config", "Defoult" }
            });
        }

        [NonAction]
        private async Task TakeUserConfigPutInCookie(UserLoginViewModel model)
        {
            Dictionary<String, String> Configuration = await _userConfigRepositore.GetUserConfig(model.Email);

            foreach (var Key in Configuration.Keys)
            {
                HttpContext.Response.Cookies.Append(Key, Configuration[Key]);
            }
        }

    }
}