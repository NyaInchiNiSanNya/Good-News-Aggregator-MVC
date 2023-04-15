using AutoMapper;
using Business_Logic.Models.UserSettings;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;

namespace MVC.MappingProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;
            
            CreateMap<User, UserRegistrationDTO>().ReverseMap();
            CreateMap<UserRegistrationViewModel, UserRegistrationDTO>().ReverseMap();

            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<UserLoginViewModel, UserLoginDTO>().ReverseMap();

            CreateMap<User, UserDTO>();
        }
    }
}
