using System.Net;
using AutoMapper;
using Business_Logic.Models.UserSettings;
using Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MVC.Filters.Validation;
using Repositores;
using UserConfigRepositores;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MVC.Filters.Validation.SettingFilter;

namespace Business_Logic.Controllers
{
     [Authorize(Roles = "Admin")]
     public class SettingsController : Controller
    {
        private readonly IUserInfoAndSettingsService _userConfigService;
        private readonly IMapper _mapper;


        public SettingsController
        (IUserInfoAndSettingsService userConfigService,
            IMapper mapper)
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
        }
        
        

        [HttpPost]
        [SettingsValidationFilter]
        public async Task<IActionResult> SetNewInfoConfig([FromForm] UserSettingsViewModel infoSettingsView)
        {
          
                await _userConfigService.SetNewUserInfoAsync(_mapper.Map<GetUserInfoWithSettingsDTO>(infoSettingsView));

                return RedirectToAction("GetInfoConfig");
                

        }


        [HttpGet]
        public async Task<IActionResult> GetInfoConfig()
        {
            //достать информацию о юзере(Email)

            GetUserInfoWithSettingsDTO infoSettings =
                    await _userConfigService.GetUserInformationAsync("Demes@mail.ru");

            if (infoSettings is not null)
            {
                //куда нибудь засунуть настройки(куки) 
                return  View("Settings",_mapper.Map<UserSettingsViewModel>(infoSettings));
            }

            return null;
        }

    }
}
