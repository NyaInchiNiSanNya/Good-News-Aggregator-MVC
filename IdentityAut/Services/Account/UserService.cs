
using System.Net.NetworkInformation;
using Abstract;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Account;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.EntityFrameworkCore;

namespace Services.Account
{
    public class UserService: IUserService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;


        public UserService(IMapper mapper, IRoleService roleService, IUnitOfWork unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;

            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _mapper = mapper;

            if (roleService is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _roleService = roleService;

        }

        public async Task<List<UserDTO>> GetAllUsersWithRolesAsync()
        {

            List<User> users = await _unitOfWork.Users.GetAsQueryable()
                .AsNoTracking().ToListAsync();
            
            if (users is not null)
            {
                List<UserDTO> usersList = new List<UserDTO>();

                foreach (var userDto in users)
                {
                    usersList.Add(_mapper.Map<UserDTO>(userDto));
                }

                foreach (var userDto in usersList)
                {
                    List<UserRole> Roles = await _roleService.GetUserRolesByUserIdAsync(userDto.Id);
                    
                    userDto.Roles = new List<String>();
                    
                    foreach (UserRole Role in Roles)
                    {
                        userDto.Roles.Add(Role.Role);
                    }
                }

                return usersList;
            }

            return null;

        }
    }
}
