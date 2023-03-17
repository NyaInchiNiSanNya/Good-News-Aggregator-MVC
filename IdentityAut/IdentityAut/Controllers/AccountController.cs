using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositores;
using Services.Account;
using UserConfigRepositores;
using Microsoft.Extensions.Configuration;
using Core.DTOs;
using Business_Logic.Controllers.HelperClasses;

namespace Business_Logic.Controllers
{
    public class AccountController : Controller
    {

        private readonly IIdentityService _IdentityService;
        private readonly IUserInfoAndSettingsService _userConfigService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController
            (IIdentityService identityService,
                IUserInfoAndSettingsService userConfigService,
                IHttpContextAccessor httpContextAccessor)
        {
            if (identityService is null)
            {
                throw new NullReferenceException();

            }
            _IdentityService = identityService;


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
        }


        public async Task<IActionResult> CheckUserExist(string Email)
        {

            if (await _IdentityService.isUserExist(Email))
            {
                return Ok(false);
            }
            return Ok(true);

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
            var validationResult = await AccountValidationHelper
                .AccountRegistrationValidator(model, _IdentityService);

            if (validationResult.IsValid)
            {
                //создание пользователя в другой бд
                await RegistratNewUser(model);
                return RedirectToAction("Login", "Account");
            }

            GetErros(validationResult);

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            var validationResult = await AccountValidationHelper
                .AccountLoginValidator(model, _IdentityService);

            if (validationResult.IsValid)
            {
                await Cookie.SettingsAndInfoPutIn(
                    _httpContextAccessor,
                    await _userConfigService.GetUserInformation(model.Email));

                return RedirectToAction("Start", "Home");
            }

            GetErros(validationResult);

            return View(model);
        }



        [NonAction]
        private async Task GetErros
            (FluentValidation.Results.ValidationResult BadResult)
        {
            foreach (var Errors in BadResult.Errors)
            {
                ModelState.AddModelError(Errors.PropertyName, Errors.ErrorMessage);
            }
        }

        //Зачем я это сделал?
        public async Task<IActionResult> LogOut()
        {
            //Response.Cookies.Append("name", "", new CookieOptions()
            //{
            //    Expires = DateTime.Now.AddDays(-1)
            //});
            //Response.Cookies.Append("email", "", new CookieOptions()
            //{
            //    Expires = DateTime.Now.AddDays(-1)
            //});
            //Response.Cookies.Append("theme", "", new CookieOptions()
            //{
            //    Expires = DateTime.Now.AddDays(-1)
            //});
            await _IdentityService.IdLogout();
            return RedirectToAction("Start", "Home");
        }



        [NonAction]
        private async Task RegistratNewUser(UserRegistrationViewModel model)
        {
            await _userConfigService.Registration(new GetUserInfoWithSettingsDTO()
            {
                Name = model.Name,
                Email = model.Email,
            });
        }

    }
}