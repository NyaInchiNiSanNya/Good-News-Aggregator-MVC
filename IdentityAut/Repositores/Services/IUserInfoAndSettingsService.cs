using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Account;
using Microsoft.AspNetCore.Http;

namespace UserConfigRepositores
{
    public interface IUserInfoAndSettingsService
    {

        public Task<userInfoWithSettingsDTO> GetUserInformationAsync(String Email);
        
        public Task SetNewUserInfoAsync(userInfoWithSettingsDTO userInfoWithSettingsDto, String Email);

        public Task SetNewProfilePictureByNameAsync(String userPicture, String Email);
        
        public Task<Int32> GetUserArticleRateFilter(String Email);
    }
}
