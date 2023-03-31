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

        public Task<GetUserInfoWithSettingsDTO> GetUserInformationAsync(String Email);
        
        public Task SetNewUserInfoAsync(GetUserInfoWithSettingsDTO getUserInfoWithSettingsDtOmodel);
    }
}
