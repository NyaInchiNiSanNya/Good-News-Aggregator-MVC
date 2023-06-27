using Core.DTOs.Account;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            if (!HttpContext.User.Identity!.Name.IsNullOrEmpty())
            {
                await _serviceFactory.CreateUserConfigService()
                .SetNewUserInfoAsync(
                    _serviceFactory.CreateMapperService().Map<userInfoWithSettingsDTO>(infoSettingsView)
                    , HttpContext.User.Identity.Name);


                HttpContext.Response.Cookies.Delete("theme");

                return RedirectToAction("GetInfoConfig");
            }
            else
            {
                return BadRequest("Пользователь не установлен");
            }

        }



        [HttpGet]
        public async Task<IActionResult> GetInfoConfig()
        {
            if (!HttpContext.User.Identity!.Name.IsNullOrEmpty())
            {
                userInfoWithSettingsDTO? infoSettings =
                    await _serviceFactory.CreateUserConfigService()
                        .GetUserInformationAsync(HttpContext.User.Identity.Name);


                if (infoSettings is not null)
                {
                    return View("Settings", _serviceFactory
                        .CreateMapperService().Map<ShowUserInfoAndConfigViewModel>(infoSettings));
                }

                return NotFound();
            }
            else
            {
                return BadRequest("Пользователь не установлен");
            }
        }


        public async Task<IActionResult> IsThemeExist(String theme)
        {

            return Ok(await _serviceFactory
                .CreateThemeService()
                .IsThemeExistByNameAsync(theme));
        }

        [HttpPost]
        public async Task<IActionResult> SetNewUserByteArrayPicture([FromBody] String userPicture)
        {
            if (!HttpContext.User.Identity!.Name.IsNullOrEmpty())
            {
                await _serviceFactory.CreateUserConfigService()
                    .SetNewProfilePictureByNameAsync(userPicture, HttpContext.User.Identity.Name!);

                return Ok();
            }
            else
            {
                return BadRequest("Пользователь не установлен");
            }
        }
    }
}
