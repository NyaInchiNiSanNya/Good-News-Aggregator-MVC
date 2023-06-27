
using System.Net.NetworkInformation;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Account;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Account
{
    public class UserService: IUserService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;


        public UserService(IMapper mapper, IRoleService roleService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));

        }

        public async Task<List<UserDto>?> GetAllUsersWithRolesAsync()
        {
            Log.Warning("Admin requested a list of users ");

            List<UserDto> users = await _unitOfWork.Users.GetAsQueryable()
                .Select(x=> _mapper.Map<UserDto>(x))
                .AsNoTracking().ToListAsync();
            
            if (users.Count>0)
            {
                foreach (var userDto in users)
                {
                    List<UserRole>? roles = await _roleService.GetUserRolesByUserIdAsync(userDto.Id);

                    if (roles != null)
                    {
                        userDto.Roles = new List<String>();

                        foreach (UserRole role in roles)
                        {
                            userDto.Roles.Add(role.Role);
                        }
                    }
                }

                return users;
            }

            return null;

        }
    }
}
