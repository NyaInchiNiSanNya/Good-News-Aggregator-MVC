using Entities_Context.Entities.UserNews;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using IServices;
using Core.DTOs.Account;
using Core.DTOs;
using System.Drawing;
using IServices.Services;
using Serilog;

namespace Services.Account
{
    public class SettingsService : ISettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IUiThemeService _uiThemeService;

        public SettingsService(
            IUnitOfWork unitOfWork
        , IMapper mapper, IUiThemeService uiThemeService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _uiThemeService = uiThemeService ?? throw new ArgumentNullException(nameof(uiThemeService));
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
        
        public async Task<userInfoWithSettingsDTO> GetUserInformationAsync(String email)
        {

            User? userModel = await _unitOfWork.Users.FindBy(x => x.Email.Equals(email)).FirstOrDefaultAsync();

            if (userModel is not null)
            {
                if (userModel.ProfilePicture != null)
                {
                    userInfoWithSettingsDTO model = new userInfoWithSettingsDTO()
                    {
                        Name = userModel.Name,
                        Theme = await _uiThemeService.GetThemeNameByIdAsync(userModel.ThemeId),
                        AllThemes = await _uiThemeService.GetAllThemesAsync(),
                        PositiveRate = userModel.PositiveRate,
                        PositiveRateFilter = userModel.PositiveRateFilter,
                        ProfilePicture = new PictureBase64EncoderDecoder().PictureDecoder(userModel.ProfilePicture)
                    };

                    return model;
                }
            }

            Log.Warning("User with email {0} is not found", email);

            return null;
        }

        private Boolean IsPictureValid(String userPicture)
        {
            PictureBase64EncoderDecoder encoder = new PictureBase64EncoderDecoder();

            byte[]? pictureByteArray = new PictureBase64EncoderDecoder().PictureDecoder(userPicture);

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

        public async Task SetNewProfilePictureByNameAsync(String userPicture, String email)
        {
            if (!IsPictureValid(userPicture))
            {
                Log.Warning("User with email {0} unsuccessfully uploaded an image", email);
                return;
            }

            User? user = await _unitOfWork.Users.FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync();


            if (user is not null && IsValidBase64String(userPicture))
            {
                await _unitOfWork.Users.PatchAsync(user.Id, new List<PatchDto>
                    {
                        new PatchDto { PropertyName = "ProfilePicture", PropertyValue = userPicture }
                    });
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                Log.Warning("User with email {0} unsuccessfully uploaded an image", email);
            }


        }

        public async Task<Boolean> SetNewUserInfoAsync(userInfoWithSettingsDTO userInfoWithSettingsDto, String? email)
        {

            User? User = await _unitOfWork.Users.FindBy(x => x.Email.Equals(email)).FirstOrDefaultAsync();

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

                Log.Information("User with email {0} changed the settings", email);
                
                return true;
            }
            else
            {
                Log.Warning("User with email {0} is not found", nameof(email));
                return false;
            }
        }

        public async Task<Int32> GetUserArticleRateFilter(String email)
        {
            return await _unitOfWork.Users.GetAsQueryable().Where(user => user.Email == email)
                .Select(user => user.PositiveRateFilter).FirstOrDefaultAsync();
        }

    }
}
