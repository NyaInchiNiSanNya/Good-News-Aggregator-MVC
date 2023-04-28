using AutoMapper;
using Business_Logic.Models.UserSettings;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;

namespace MVC.MappingProfiles
{
    public class SettingsProfile:Profile
    {
        public SettingsProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<userInfoWithSettingsDTO, User>().ReverseMap();

            CreateMap<userInfoWithSettingsDTO, NewUserSettingsViewModel>().ReverseMap();

            CreateMap<ShowUserInfoAndConfigViewModel, userInfoWithSettingsDTO>().ReverseMap();
        }
    }

}
