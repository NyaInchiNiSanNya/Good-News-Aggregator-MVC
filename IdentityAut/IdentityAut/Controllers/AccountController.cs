using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositores;
using UserConfigRepositores;
using Microsoft.Extensions.Configuration;
using Business_Logic.Controllers.HelperClasses;
using AutoMapper;
using Core.DTOs.Account;
using MVC.Filters.Validation;
using Services.Account;

namespace Business_Logic.Controllers
{
    public class AccountController : Controller
    {

        private readonly IIdentityService _IdentityService;
        private readonly IMapper _mapper;

        public AccountController
            (IIdentityService identityService,
                IMapper mapper
                )
        {
            if (identityService is null)
            {
                throw new NullReferenceException(nameof(identityService));

            }
            _IdentityService = identityService;


            if (mapper is null)
            {
                throw new NullReferenceException(nameof(mapper));

            }
            _mapper = mapper;
        }


        public async Task<IActionResult> CheckUserLoginExist(String Email)
        {

            if (await _IdentityService.isUserExist(Email))
            {
                return Ok(true);
            }
            return Ok(false);

        }

        public async Task<IActionResult> CheckUserRegistrationExist(String Email)
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
        [RegistrationValidationFilter]
        public async Task<IActionResult> Registration
            ([FromForm] UserRegistrationViewModel model)
        {
            await _IdentityService.Registration(
                    _mapper.Map<UserRegistrationDTO>(model));
            
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromForm] UserLoginViewModel model)
        {
           
                //положить в куки или т.п.
                return RedirectToAction("GetInfoConfig", "Settings");
        }

        [NonAction]

        public async Task<IActionResult> LogOut()
        {
            await _IdentityService.IdLogout();
            return RedirectToAction("Start", "Home");
        }

    }
}