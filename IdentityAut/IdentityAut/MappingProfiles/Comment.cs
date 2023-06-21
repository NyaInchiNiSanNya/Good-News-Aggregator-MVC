using AutoMapper;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;

namespace MVC.MappingProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<CommentDto, Comment>().ReverseMap().ForMember(dto => dto.UserName,
                opt
                    => opt.MapFrom(
                        user
                            => user.User.Name))
                .ForMember(dto => dto.UserPicture,
                opt
                    => opt.MapFrom(
                        user
                            => user.User.ProfilePicture));
        }
    }
}
