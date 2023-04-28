using System.Net;
using AutoMapper;
using Business_Logic.Models.UserSettings;
using Core.DTOs.Account;
using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.IdentityModel.Tokens;
using MVC.ControllerFactory;
using MVC.Filters.Validation;
using Repositores;
using Services.Account;
using UserConfigRepositores;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Business_Logic.Controllers
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
        [SettingsValidationFilterAttribute]
        public async Task<IActionResult> SetNewInfoConfig([FromForm] NewUserSettingsViewModel infoSettingsView)
        {

            await _serviceFactory.createUserConfigService()
                .SetNewUserInfoAsync(
                    _serviceFactory.createMapperService().Map<userInfoWithSettingsDTO>(infoSettingsView)
                    , HttpContext.User.Identity.Name);
            
            return RedirectToAction("GetInfoConfig");
                

        }

        [Route("Set")]
        [HttpGet]
        public async Task<IActionResult> GetInfoConfig()
        {
            userInfoWithSettingsDTO infoSettings =
                    await _serviceFactory.createUserConfigService()
                        .GetUserInformationAsync(HttpContext.User.Identity.Name);

            
            if (infoSettings is not null)
            {
                return  View("Settings",_serviceFactory
                    .createMapperService().Map<ShowUserInfoAndConfigViewModel>(infoSettings));
            }

            return null;
        }


        public async Task<IActionResult> isThemeExist(String Theme)
        {

            return Ok(await _serviceFactory
                .createThemeService()
                .IsThemeExistByNameAsync(Theme));
        }
        [HttpPost]
        public async Task<IActionResult> GetUserByteArrayPicture([FromBody] String userPicture)
        {
            if (HttpContext.User.Identity.Name is not null)
            {
                await _serviceFactory.createUserConfigService()
                    .SetNewProfilePictureByNameAsync(userPicture, HttpContext.User.Identity.Name);
            }

            return Ok();
        }
    }
}
