using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;

namespace UserConfigRepositores
{
    public interface IUserInfoAndSettingsService
    {
        public Task Registration(GetUserInfoWithSettingsDTO getUserInfoWithSettingsDtOmodel);

        public Task<Boolean> isExist(String Email);

        public Task<GetUserInfoWithSettingsDTO> GetUserInformation(String Email);
        
        public Task SetNewUserInfo(GetUserInfoWithSettingsDTO getUserInfoWithSettingsDtOmodel);
    }
}
