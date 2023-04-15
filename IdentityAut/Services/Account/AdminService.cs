
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Account;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.EntityFrameworkCore;

namespace Services.Account
{
    public class AdminService: IAdminService
    {

        private readonly UserArticleContext _userContext;

        private readonly IMapper _Mapper;
        private readonly IRoleService _roleService;


        public AdminService(UserArticleContext userContext
            , IMapper mapper, IRoleService roleService)
        {
            if (userContext is null)
            {
                throw new ArgumentNullException(nameof(userContext));
            }

            _userContext = userContext;

            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _Mapper = mapper;

            if (roleService is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _roleService = roleService;

        }

        public async Task<List<UserDTO>> GetAllUsers()
        {

            List<User> Users = await _userContext.Users.AsNoTracking().ToListAsync();

            List<UserDTO> UsersList=new List<UserDTO>();

            foreach (var userDto in Users)
            {
                UsersList.Add(_Mapper.Map<UserDTO>(userDto));
            }

            foreach (var UserDTO in UsersList)
            {
                List<UserRole> Roles = await _roleService.GetUserRolesByUserId(UserDTO.Id);
                UserDTO.Roles = new List<String>();
                foreach (UserRole Role in Roles)
                {
                    UserDTO.Roles.Add(Role.Role);
                }
            }
            

            return UsersList;
        }
    }
}
