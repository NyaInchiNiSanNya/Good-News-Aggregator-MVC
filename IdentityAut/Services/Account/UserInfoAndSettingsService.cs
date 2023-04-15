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
using Microsoft.IdentityModel.Tokens;
using Entities_Context;
using IServices;
using Core.DTOs.Account;

namespace Services.Account
{
    public class UserInfoAndSettingsService : IUserInfoAndSettingsService
    {
        private readonly UserArticleContext _UserInfoContext;

        private readonly IMapper _mapper;

        private readonly IUiThemeService _uiThemeService;

        public UserInfoAndSettingsService(UserArticleContext userInfoContext
        , IMapper mapper, IUiThemeService uiThemeService)
        {
            if (userInfoContext is null)
            {
                throw new ArgumentNullException(nameof(userInfoContext));
            }

            _UserInfoContext = userInfoContext;

            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _mapper = mapper;
            
            if (uiThemeService is null)
            {
                throw new ArgumentNullException(nameof(uiThemeService));
            }
            _uiThemeService = uiThemeService;
        }

        public async Task<GetUserInfoWithSettingsDTO> GetUserInformationAsync(String Email)
        {
            User UserModel= await _UserInfoContext.Users
                .AsNoTracking()
                .Where(x => x.Email.Equals(Email))
                .SingleOrDefaultAsync();

            if (UserModel is not null)
            {
                GetUserInfoWithSettingsDTO model = new GetUserInfoWithSettingsDTO()
                {
                    Name = UserModel.Name,
                    Theme = await _uiThemeService.GetThemeNameByIdAsync(UserModel.ThemeId),
                    AllThemes = await _uiThemeService.GetAllThemesAsync(),
                    PositiveRate = UserModel.PositiveRate,
                    PositiveRateFilter = UserModel.PositiveRateFilter,
                    ProfilePicture = Convert.FromBase64String(UserModel.ProfilePicture)
                };
                return model;
            }

            return null;
        }

        public async Task SetNewUserInfoAsync(GetUserInfoWithSettingsDTO getUserInfoAndSettingsDtOmodel,String Email)
        {

            User User = await _UserInfoContext.Users
                .Where(x => x.Email.Equals(Email))
                .SingleOrDefaultAsync();

            User.ThemeId =await _uiThemeService.GetIdThemeByStringAsync(getUserInfoAndSettingsDtOmodel.Theme);

            
            User.Name = getUserInfoAndSettingsDtOmodel.Name;

            User.PositiveRateFilter = getUserInfoAndSettingsDtOmodel.PositiveRateFilter;

            //User.ProfilePicture = BitConverter.ToString(getUserInfoAndSettingsDtOmodel.ProfilePicture);

            await _UserInfoContext.SaveChangesAsync();
        }

    }
}
