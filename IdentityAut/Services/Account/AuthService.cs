using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Abstract;
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
    public sealed class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IUiThemeService _uiTheme;

        private readonly IRoleService _role;

        public AuthService(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IUiThemeService uiTheme, 
            IRoleService role)
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

            if (uiTheme is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _uiTheme = uiTheme;

            if (role is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _role = role;
        }

        public async Task<Boolean> isUserExistAsync(String Email)
        {
            return await _unitOfWork.Users
                .FindBy(x => x.Email.Equals(Email))
                .FirstOrDefaultAsync() is not null;
        }


        public async Task<Boolean> RegistrationAsync(UserRegistrationDTO modelDTO)
        {

            if(!await isUserExistAsync(modelDTO.Email)){

                User newUser = _mapper.Map<User>(modelDTO);

                newUser.ThemeId =await _uiTheme.GetIdDefaultThemeAsync();
                
                newUser.Password = MakeHash(modelDTO.Password);

                newUser.Created=DateTime.Now;

                newUser.ProfilePicture =
                    Convert.ToBase64String(
                        await File.ReadAllBytesAsync(
                            @"C:\\Users\\User\\Desktop\\ASP-Project\\ASProject\\IdentityAut\\IdentityAut\\wwwroot\\images\\defaultImage3.jpg"));

                await _unitOfWork.Users.AddAsync(newUser);


                await _unitOfWork.SaveChangesAsync();

                UserRole Role = await _role.GetDefaultRoleAsync();


                if (Role is null)
                {
                    throw new ArgumentException("Can't create role");
                }


                UsersRoles newUserRole=new UsersRoles()
                {
                    RoleId =Role.Id,

                    UserId= (await _unitOfWork.Users.FindBy(x=>x.Email.Equals(modelDTO.Email)).FirstOrDefaultAsync()).Id
                };

                await _unitOfWork.UsersRoles.AddAsync(newUserRole);
                await _unitOfWork.SaveChangesAsync();


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
            User? сheckUser = _unitOfWork.Users.FindBy(x => x.Email.Equals(modelDTO.Email)).FirstOrDefault();

            if (сheckUser is not null
                && CheckPassword(modelDTO.Password, сheckUser.Password))
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
