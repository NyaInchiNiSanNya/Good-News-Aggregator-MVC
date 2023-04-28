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
using System.Collections;
using System.Drawing;

namespace Services.Account
{
    public class UserInfoAndSettingsService : IUserInfoAndSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IUiThemeService _uiThemeService;

        public UserInfoAndSettingsService(
            IUnitOfWork unitOfWork, UserArticleContext userInfoContext
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

        bool IsValidBase64String(String base64String)
        {
            try
            {
                byte[] data = Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public async Task<userInfoWithSettingsDTO> GetUserInformationAsync(String Email)
        {

            User? UserModel = await _unitOfWork.Users.FindBy(x => x.Email.Equals(Email)).FirstOrDefaultAsync();

            if (UserModel is not null)
            {

                userInfoWithSettingsDTO model = new userInfoWithSettingsDTO()
                {
                    Name = UserModel.Name,
                    Theme = await _uiThemeService.GetThemeNameByIdAsync(UserModel.ThemeId),
                    AllThemes = await _uiThemeService.GetAllThemesAsync(),
                    PositiveRate = UserModel.PositiveRate,
                    PositiveRateFilter = UserModel.PositiveRateFilter,
                    ProfilePicture = new PictureBase64EncoderDecoder().PictureDecoder(UserModel.ProfilePicture)
                };

                return model;
            }

            return null;
        }

        private Boolean IsPictureValid(String userPicture)
        {
            PictureBase64EncoderDecoder encoder = new PictureBase64EncoderDecoder();

            Byte[] pictureByteArray = new PictureBase64EncoderDecoder().PictureDecoder(userPicture);

            using (MemoryStream ms = new MemoryStream(pictureByteArray))
            {
                Image image = Image.FromStream(ms);

                if (pictureByteArray.Length> 2097152)
                {
                    return false;
                }
                
                Int32 width = image.Width;
                Int32 height = image.Height;

                if (!((width > 200 && height > 200) & (width < 1920 && height < 1080)))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task SetNewProfilePictureByNameAsync(String userPicture, String name)
        {
            if (!IsPictureValid(userPicture))
            {
                return;
            }

            User? user = await _unitOfWork.Users.FindBy(user => user.Email.Equals(name)).FirstOrDefaultAsync();


            if (user is not null && IsValidBase64String(userPicture))
            {
                await _unitOfWork.Users.PatchAsync(user.Id, new List<PatchDto>
                    {
                        new PatchDto { PropertyName = "ProfilePicture", PropertyValue = userPicture }
                    });
                await _unitOfWork.SaveChangesAsync();
            }


        }

        public async Task SetNewUserInfoAsync(userInfoWithSettingsDTO userInfoWithSettingsDto, String Email)
        {

            User? User = await _unitOfWork.Users.FindBy(x => x.Email.Equals(Email)).FirstOrDefaultAsync();

            if (User is not null)
            {
                var patchDtos = new List<PatchDto> {

                    new PatchDto { PropertyName = "Name", PropertyValue = userInfoWithSettingsDto.Name },

                    new PatchDto { PropertyName = "ThemeId",  PropertyValue = await _uiThemeService
                        .GetIdThemeByStringAsync(userInfoWithSettingsDto.Theme) },

                    new PatchDto { PropertyName = "PositiveRateFilter", PropertyValue = userInfoWithSettingsDto.PositiveRateFilter }
                };

                await _unitOfWork.Users.PatchAsync(User.Id, patchDtos);

                await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
