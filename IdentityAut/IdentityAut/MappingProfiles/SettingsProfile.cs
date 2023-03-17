using AutoMapper;
using Business_Logic.Models.UserSettings;
using Core.DTOs;
using Entities_Context.Entities.UserNews;

namespace MVC.MappingProfiles
{
    public class SettingsProfile:Profile
    {
        public SettingsProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<GetUserInfoWithSettingsDTO, UserInformation>().ReverseMap();

            CreateMap<GetUserInfoWithSettingsDTO, UserSettingsViewModel>().ReverseMap();
        }
    }

}
