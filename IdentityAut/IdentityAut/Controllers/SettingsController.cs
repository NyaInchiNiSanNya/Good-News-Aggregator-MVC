using System.Net;
using AutoMapper;
using Business_Logic.Controllers.HelperClasses;
using Business_Logic.Models.UserSettings;
using Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Repositores;
using Services.Account;
using UserConfigRepositores;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Business_Logic.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserInfoAndSettingsService _userConfigService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;


        public SettingsController
        (IUserInfoAndSettingsService userConfigService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {

            if (userConfigService is null)
            {
                throw new NullReferenceException();

            }
            _userConfigService = userConfigService;
            if (httpContextAccessor is null)
            {
                throw new NullReferenceException();

            }
            _httpContextAccessor = httpContextAccessor;
            if (httpContextAccessor is null)
            {
                throw new NullReferenceException();

            }

            _mapper = mapper;
        }


        public async Task<IActionResult> CheckUserExist(string Email)
        {

            if (!await _userConfigService.isExist(Email))
            {
                return Ok(false);
            }
            return Ok(true);

        }

        [HttpPost]
        public async Task<IActionResult> SetNewInfoConfig(UserSettingsViewModel infoSettingsView)
        {
            var validationResult = await AccountValidationHelper
                .InfoSettingsValidator(infoSettingsView, _userConfigService);

            if (validationResult.IsValid)
            {
                await _userConfigService.SetNewUserInfo(_mapper.Map<GetUserInfoWithSettingsDTO>(infoSettingsView));

                //await Cookie.SettingsAndInfoPutIn(
                //    _httpContextAccessor,
                //    _mapper.Map<GetUserInfoWithSettingsDTO>(infoSettingsView));

                return RedirectToAction("GetInfoConfig");
            }

            foreach (var Error in validationResult.Errors)
            {
                ModelState.AddModelError(Error.PropertyName, Error.ErrorMessage);
            }

            return View("Settings",infoSettingsView);

        }


        [HttpGet]//Или лучше взять настройки из куки? 
        public async Task<IActionResult> GetInfoConfig()
        {
            if (HttpContext.Request.Cookies.ContainsKey("email"))
            {
                var value = _httpContextAccessor.HttpContext.Request.Cookies["email"];

                GetUserInfoWithSettingsDTO infoSettings =
                    await _userConfigService.GetUserInformation(value);

                if (infoSettings != null)
                {
                    return View("Settings", _mapper.Map<UserSettingsViewModel>(infoSettings));
                }
            }
            //сделать логаут или создать новые куки?
            return NotFound("no cookies found");
        }

    }
}
