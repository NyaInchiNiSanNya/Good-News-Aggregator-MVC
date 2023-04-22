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
        }
    }

}