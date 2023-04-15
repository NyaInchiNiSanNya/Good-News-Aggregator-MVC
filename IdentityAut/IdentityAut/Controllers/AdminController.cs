using AutoMapper;
using IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositores;

namespace MVC.Controllers
{
    [Authorize(Roles="SuperAdmin")]
    public class AdminController : Controller
    {

        private readonly IAdminService _AdminService;
        private readonly IMapper _mapper;

        public AdminController
        (IAdminService adminService,
            IMapper mapper
        )
        {
            if (adminService is null)
            {
                throw new NullReferenceException(nameof(adminService));

            }
            _AdminService = adminService;


            if (mapper is null)
            {
                throw new NullReferenceException(nameof(mapper));

            }
            _mapper = mapper;


        }

        [HttpGet]
        public async  Task<IActionResult> UserList()
        {
            return View("AllUsers", await _AdminService.GetAllUsers());
        }
    }
}
