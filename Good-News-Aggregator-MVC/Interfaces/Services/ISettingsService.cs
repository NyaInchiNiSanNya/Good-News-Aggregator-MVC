using Core.DTOs.Account;

namespace IServices.Services
{
    public interface ISettingsService
    {

        public Task<userInfoWithSettingsDTO?> GetUserInformationAsync(String email);
        
        public Task<Boolean> SetNewUserInfoAsync(userInfoWithSettingsDTO userInfoWithSettingsDto, String? email);

        public Task SetNewProfilePictureByNameAsync(String userPicture, String email);
        
        public Task<Int32> GetUserArticleRateFilter(String email);
    }
}
