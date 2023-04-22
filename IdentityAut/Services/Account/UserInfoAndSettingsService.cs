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
using Abstract;
using Core.DTOs;

namespace Services.Account
{
    public class UserInfoAndSettingsService : IUserInfoAndSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IUiThemeService _uiThemeService;

        public UserInfoAndSettingsService(
            IUnitOfWork unitOfWork,UserArticleContext userInfoContext
        , IMapper mapper, IUiThemeService uiThemeService)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(userInfoContext));
            }

            _unitOfWork = unitOfWork;

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
            User? UserModel= await _unitOfWork.Users.FindBy(x=>x.Email.Equals(Email)).FirstOrDefaultAsync();

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

            User? User = await _unitOfWork.Users.FindBy(x => x.Email.Equals(Email)).FirstOrDefaultAsync();
            
            if (User is not null)
            {
                var patchDtos = new List<PatchDto> {
                    new PatchDto { PropertyName = "Name", PropertyValue = getUserInfoAndSettingsDtOmodel.Name },
                    new PatchDto { PropertyName = "ThemeId",  PropertyValue = await _uiThemeService
                        .GetIdThemeByStringAsync(getUserInfoAndSettingsDtOmodel.Theme) },
                    new PatchDto { PropertyName = "PositiveRateFilter", PropertyValue = getUserInfoAndSettingsDtOmodel.PositiveRateFilter }
                };
                await _unitOfWork.Users.PatchAsync(User.Id, patchDtos);

                //User.ProfilePicture = BitConverter.ToString(getUserInfoAndSettingsDtOmodel.ProfilePicture);

                await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
