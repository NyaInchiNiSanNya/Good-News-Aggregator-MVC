using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositores;
using UserConfigRepositores;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Core.DTOs.Account;
using MVC.Filters.Validation;
using Services.Account;
using IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Serilog;

namespace Business_Logic.Controllers
{
    public class AccountController : Controller
    {

        private readonly IIdentityService _IdentityService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public AccountController
            (IIdentityService identityService,
                IMapper mapper,
                IRoleService roleService
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

            if (roleService is null)
            {
                throw new NullReferenceException(nameof(mapper));

            }
            _roleService = roleService;

            
        }


        public async Task<IActionResult> CheckUserLoginExist(String Email)
        {

            return Ok(await _IdentityService.isUserExistAsync(Email));

        }

        public async Task<IActionResult> CheckUserRegistrationExist(String Email)
        {
            return Ok(!await _IdentityService.isUserExistAsync(Email));

        }


        [HttpGet]
        public IActionResult Registration()
        {
            return View("Registration");
        }

        [HttpGet]
        public IActionResult Login()
        {

            if (HttpContext.Request.Query.ContainsKey("ReturnUrl"))
            {
                var url = new UserLoginViewModel()
                {
                    ReturnUrl = HttpContext.Request.Query["ReturnUrl"]
                };
                return View(url);
            }
            return View();
        }


        [HttpPost]
        [RegistrationValidationFilter]
        public async Task<IActionResult> Registration
            ([FromForm] UserRegistrationViewModel model)
        {
            if (await _IdentityService.RegistrationAsync(
                    _mapper.Map<UserRegistrationDTO>(model)))
            {
                return RedirectToAction("Login", "Account");
            }

            return Conflict("Пользователь уже существует");
        }



        [HttpPost]
        [ServiceFilter(typeof(LoginValidationFilterAttribute))]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {

            const string authType = "Application Cookie";
            
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Email),
                };


            var roles = (await _roleService.GetUserRolesByUserName(model.Email));

            if (roles is null)
            {
                throw new ArgumentException("Incorrect user or role", nameof(model));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Role));
            }

            var identity = new ClaimsIdentity(claims,
                authType,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            
            Log.Information("User {0} in the system, ip:{1}",model.Email
                , HttpContext.Connection.RemoteIpAddress?.ToString());
            
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return LocalRedirect(model.ReturnUrl);
            }

            return RedirectToAction("GetInfoConfig", "Settings");
        }


        public async Task<IActionResult> LogOut()
        {
            await _IdentityService.IdLogoutAsync();
            return RedirectToAction("Start", "Home");
        }

    }
}