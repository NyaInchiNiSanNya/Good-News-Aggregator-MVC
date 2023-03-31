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
        private readonly UserArticleContext _UserInfoContext;

        private readonly IMapper _mapper;

        public UserInfoAndSettingsService(UserArticleContext userInfoContext
        , IMapper mapper)
        {
            if (userInfoContext is null)
            {
                throw new ArgumentNullException(nameof(userInfoContext));
            }

            _UserInfoContext = userInfoContext;

            if (userInfoContext is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _mapper = mapper;
        }

        public async Task<GetUserInfoWithSettingsDTO> GetUserInformationAsync(String Email)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _UserInfoContext.Users
                .Where(x => x.Email.Equals(Email))
                .AsNoTracking().ProjectTo<GetUserInfoWithSettingsDTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.

        }

        public async Task SetNewUserInfoAsync(GetUserInfoWithSettingsDTO getUserInfoAndSettingsDtOmodel)
        {

            User User = await _UserInfoContext.Users
                .Where(x => x.Email == getUserInfoAndSettingsDtOmodel.Email)
                .FirstOrDefaultAsync();

            User.ThemeId = await _UserInfoContext.Themes
                .Where(x => x.Theme.Equals(getUserInfoAndSettingsDtOmodel.Theme))
                .Select(x=>x.Id)
                .FirstOrDefaultAsync();

            User.Name = getUserInfoAndSettingsDtOmodel.Name;
            User.PositiveRateFilter = getUserInfoAndSettingsDtOmodel.PositiveRateFilter;
            User.ProfilePicture = getUserInfoAndSettingsDtOmodel.ProfilePicture;

            await _UserInfoContext.SaveChangesAsync();
        }

    }
}
