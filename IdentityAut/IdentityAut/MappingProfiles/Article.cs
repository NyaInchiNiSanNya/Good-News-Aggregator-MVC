using AutoMapper;
using Business_Logic.Models.UserSettings;
using Core.DTOs.Account;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;

namespace MVC.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<FullArticleDTO, Article>().ReverseMap();
            CreateMap<Article, AutoCompleteDataDto>()
                .ForMember(dto => dto.Label,
                    opt
                        => opt.MapFrom(
                            article
                                => article.Title))
                .ForMember(dto => dto.Value, opt => opt.MapFrom(article => article.Id)); //source -> destination
        }
    }

}