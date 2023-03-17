using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserConfigRepositores;
using Core.DTOs;
using Microsoft.IdentityModel.Tokens;
using Entities_Context;

namespace Services.Account
{
    public class UserInfoAndSettingsService : IUserInfoAndSettingsService
    {
        private readonly UserNewsContext _UserInfoContext;

        private readonly IMapper _Mapper;

        public UserInfoAndSettingsService(UserNewsContext userInfoContext
        ,IMapper mapper)
        {
            if (userInfoContext is null)
            {
                throw new ArgumentNullException();
            }

            _UserInfoContext = userInfoContext;
            
            if (userInfoContext is null)
            {
                throw new ArgumentNullException();
            }

            _Mapper = mapper;
        }

        public async Task<bool> isExist(string Email)
        {
            if (!Email.IsNullOrEmpty())
            {
                UserInformation User = new UserInformation()
                {
                    Email = Email
                };


                if (await _UserInfoContext.UserInformation
                        .Where(x => User.Email==x.Email)
                        .AsNoTracking()
                        .FirstOrDefaultAsync() is not null)
                {
                    return true;
                }
            }

            return false;
        }


        public async Task Registration(GetUserInfoWithSettingsDTO getUserInfoAndSettingsDtOmodel)
        {
            await _UserInfoContext.UserInformation.AddAsync
                (_Mapper.Map<UserInformation>(getUserInfoAndSettingsDtOmodel));

            await _UserInfoContext.SaveChangesAsync();
        }

        public async Task<GetUserInfoWithSettingsDTO> GetUserInformation(String Email)
        {
            return await _UserInfoContext.UserInformation
                .Where(x => x.Email == Email)
                        .Include(x => x.UserConfig)
                        .AsNoTracking().ProjectTo<GetUserInfoWithSettingsDTO>(_Mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
          
        }

        public async Task SetNewUserInfo(GetUserInfoWithSettingsDTO getUserInfoAndSettingsDtOmodel)
        {
            UserInformation UpdateUser = _Mapper.Map<UserInformation>(getUserInfoAndSettingsDtOmodel);
            
            UserInformation User = await _UserInfoContext.UserInformation
                .Where(x => x.Email == UpdateUser.Email)
                .Include(x => x.UserConfig)
                .FirstOrDefaultAsync();

            User = UpdateUser;
                
            await _UserInfoContext.SaveChangesAsync();
        }

    }
}
