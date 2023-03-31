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

            CreateMap<GetUserInfoWithSettingsDTO, User>().ReverseMap()
                //.
                //ForMember(
                //dest => dest.Role,
                //opt =>
                //    opt.MapFrom(src => src.Role.Role)).

                //ForMember(est => est.Theme,
                //opt =>
                //    opt.MapFrom(src => src.Theme.Theme))
                ;

            CreateMap<GetUserInfoWithSettingsDTO, UserSettingsViewModel>().ReverseMap();
        }
    }

}
