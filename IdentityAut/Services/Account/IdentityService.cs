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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositores;

namespace Services.Account
{
    public sealed class IdentityService : IIdentityService
    {

        private readonly UserArticleContext _userContext;

        private readonly IMapper _Mapper;

        public IdentityService(UserArticleContext userContext
        , IMapper mapper)
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
        }

        public async Task<Boolean> isUserExist(String Email)
        {
            if (await _userContext.Users
                    .AsNoTracking()
                    .AnyAsync(x=>x.Email.Equals(Email)))
            {
                return true;
            }

            return false;
        }


        public async Task Registration(UserRegistrationDTO modelDTO)
        {

            User newUser = _Mapper.Map<User>(modelDTO);

            newUser.ThemeId = await _userContext.Themes
                .AsNoTracking()
                .Where(x => x.Theme
                    .Equals("default"))
                .Select(x => x.Id).FirstOrDefaultAsync();
            
            newUser.RoleId = await _userContext.Roles
                .AsNoTracking()
                .Where(x => x.Role
                    .Equals("User"))
                .Select(x => x.Id).FirstOrDefaultAsync(); 

            _userContext.Users.Add(newUser);



            await _userContext.SaveChangesAsync();
        }

        
        public async Task<Boolean> Login(UserLoginDTO modelDTO)
        {
            //Баг #1 Асинхронный вызов ломает логин
            if (await _userContext.Users
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Email.Equals(modelDTO.Email)
                    && x.Password.Equals(modelDTO.Password)))
            {
                return true;
            }

            return false;

        }


        public async Task IdLogout()
        {
        }

    }
}
