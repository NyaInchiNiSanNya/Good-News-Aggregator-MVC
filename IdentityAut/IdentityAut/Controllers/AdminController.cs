using AutoMapper;
using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ControllerFactory;
using Repositores;

namespace MVC.Controllers
{
    [Authorize(Roles="SuperAdmin")]
    public class AdminController : Controller
    {

        private readonly IServiceFactory _serviceFactory;

        public AdminController
        (IServiceFactory serviceFactory)
        {
            if (serviceFactory is null)
            {
                throw new NullReferenceException(nameof(serviceFactory));
            }
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        public async  Task<IActionResult> UserList()
        {
            return View("AllUsers", await _serviceFactory
                .createAdminService()
                .GetAllUsersWithRolesAsync());
        }
    }
}
