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

            CreateMap<GetUserInfoWithSettingsDTO, User>().ReverseMap();

            CreateMap<GetUserInfoWithSettingsDTO, NewUserSettingsViewModel>().ReverseMap();

            CreateMap<ShowUserInfoAndConfigViewModel, GetUserInfoWithSettingsDTO>().ReverseMap();
        }
    }

}
