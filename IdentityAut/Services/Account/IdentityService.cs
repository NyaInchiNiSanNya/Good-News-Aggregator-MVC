using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Account;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositores;


namespace Services.Account
{
    public sealed class IdentityService : IIdentityService
    {

        private readonly UserArticleContext _userContext;

        private readonly IMapper _Mapper;
        private readonly IRoleService _roleService;
        private readonly IUiThemeService _uiTheme;

        public IdentityService(UserArticleContext userContext
        , IMapper mapper,IRoleService roleService, IUiThemeService uiTheme)
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

            _roleService=roleService;

            if (uiTheme is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _uiTheme = uiTheme;
        }

        public async Task<Boolean> isUserExistAsync(String Email)
        {
            return await _userContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Email.Equals(Email));
        }


        public async Task<Boolean> RegistrationAsync(UserRegistrationDTO modelDTO)
        {

            if(!await isUserExistAsync(modelDTO.Email)){

                User newUser = _Mapper.Map<User>(modelDTO);

                newUser.ThemeId =await _uiTheme.GetIdDefaultThemeAsync();
                
                newUser.Password = MakeHash(modelDTO.Password);

                newUser.Created=DateTime.Now;

                newUser.ProfilePicture =
                    Convert.ToBase64String(
                        await File.ReadAllBytesAsync(
                            "C:\\Users\\User\\source\\repos\\IdentityAut\\IdentityAut\\wwwroot\\images\\defaultImage3.jpg"));

                _userContext.Users.Add(newUser);


                await _userContext.SaveChangesAsync();

                UserRole Role = await _roleService.GetDefaultRole();


                if (Role is null)
                {
                    await _roleService.InitiateDefaultRolesAsync();

                    Role= await _roleService.GetDefaultRole();
                }


                UsersRoles newUserRole=new UsersRoles()
                {
                    RoleId =Role.Id,

                    UserId= await _userContext.Users.Where(x=>x.Email.Equals(modelDTO.Email))
                        .Select(x=>x.Id)
                        .SingleOrDefaultAsync()
                };

                _userContext.UsersRoles.Add(newUserRole);
                await _userContext.SaveChangesAsync();


                return true;
            }

            return false;
        }

        #region PasswordHash
        static String MakeHash(String Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password);
        }

        static Boolean CheckPassword(String Password,String PasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(Password,PasswordHash);
        }
        #endregion

        public async Task<Boolean> LoginAsync(UserLoginDTO modelDTO)
        {
            User CheckUser = _userContext.Users
                .Where(x => x.Email.Equals(modelDTO.Email)).FirstOrDefault();


            if (CheckUser is not null
                    && CheckPassword(modelDTO.Password, CheckUser.Password))
            {
                return true;
            }

            return false;

        }


        public async Task IdLogoutAsync()
        {
        }

    }
}
