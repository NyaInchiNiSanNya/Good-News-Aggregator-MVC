using Core.DTOs.Account;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ControllerFactory;
using MVC.Filters.Validation;
using MVC.Models.UserSettings;
using Serilog;


namespace MVC.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IServiceFactory _serviceFactory;

        public SettingsController
            (IServiceFactory serviceFactory)
        {

            if (serviceFactory is null)
            {
                throw new NullReferenceException(nameof(serviceFactory));
            }

            _serviceFactory = serviceFactory;
        }



        [HttpPost]
        [SettingsValidationFilter]
        public async Task<IActionResult> SetNewInfoConfig([FromForm] NewUserSettingsViewModel infoSettingsView)
        {

            await _serviceFactory.CreateUserConfigService()
                .SetNewUserInfoAsync(
                    _serviceFactory.CreateMapperService().Map<userInfoWithSettingsDTO>(infoSettingsView)
                    , HttpContext.User.Identity.Name);


            HttpContext.Response.Cookies.Delete("theme");

            return RedirectToAction("GetInfoConfig");


        }



        [HttpGet]
        public async Task<IActionResult> GetInfoConfig()
        {
            userInfoWithSettingsDTO infoSettings =
                    await _serviceFactory.CreateUserConfigService()
                        .GetUserInformationAsync(HttpContext.User.Identity.Name);

            
            if (infoSettings is not null)
            {
                return  View("Settings",_serviceFactory
                    .CreateMapperService().Map<ShowUserInfoAndConfigViewModel>(infoSettings));
            }

            return NotFound();
        }


        public async Task<IActionResult> IsThemeExist(String Theme)
        {

            return Ok(await _serviceFactory
                .CreateThemeService()
                .IsThemeExistByNameAsync(Theme));
        }

        [HttpPost]
        public async Task<IActionResult> GetUserByteArrayPicture([FromBody] String userPicture)
        {
            if (HttpContext.User.Identity.Name is not null)
            {
                await _serviceFactory.CreateUserConfigService()
                    .SetNewProfilePictureByNameAsync(userPicture, HttpContext.User.Identity.Name);
                return Ok();
            }
            else
            {
                Log.Warning("attempt to get user picture without authorization");

                return BadRequest("Пользователь не установлен");
            }
        }
    }
}
