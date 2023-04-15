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
        private readonly IUserInfoAndSettingsService _userConfigService;
        private readonly IMapper _mapper;
        private readonly IUiThemeService _uiThemeService;


        public SettingsController
        (IUserInfoAndSettingsService userConfigService,
            IMapper mapper , IUiThemeService uiThemeService)
        {

            if (userConfigService is null)
            {
                throw new NullReferenceException(nameof(userConfigService));

            }
            _userConfigService = userConfigService;


            if (mapper is null)
            {
                throw new NullReferenceException(nameof(mapper));

            }

            _mapper = mapper;

            if (uiThemeService is null)
            {
                throw new NullReferenceException(nameof(mapper));

            }

            _uiThemeService = uiThemeService;
        }
        
        

        [HttpPost]
        [SettingsValidationFilterAttribute]
        public async Task<IActionResult> SetNewInfoConfig([FromForm] NewUserSettingsViewModel infoSettingsView)
        {

            await _userConfigService.SetNewUserInfoAsync(_mapper.Map<GetUserInfoWithSettingsDTO>(infoSettingsView)
                    , HttpContext.User.Identity.Name);
            
            return RedirectToAction("GetInfoConfig");
                

        }

        [Route("Set")]
        [HttpGet]
        public async Task<IActionResult> GetInfoConfig()
        {
            GetUserInfoWithSettingsDTO infoSettings =
                    await _userConfigService.GetUserInformationAsync(HttpContext.User.Identity.Name);

            if (infoSettings is not null)
            {
                return  View("Settings",_mapper.Map<ShowUserInfoAndConfigViewModel>(infoSettings));
            }

            return null;
        }


        public async Task<IActionResult> isThemeExist(String Theme)
        {

            return Ok(await _uiThemeService.IsThemeExistByNameAsync(Theme));
        }
    }
}
