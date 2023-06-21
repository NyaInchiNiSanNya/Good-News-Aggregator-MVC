using AutoMapper;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;

namespace Web_Api_Controllers.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            SourceMemberNamingConvention = LowerUnderscoreNamingConvention.Instance;
            DestinationMemberNamingConvention = PascalCaseNamingConvention.Instance;

            CreateMap<SourceDto, Source>().ReverseMap();
            CreateMap<FullArticleDto, Article>().ReverseMap();
            CreateMap<ShortArticleDto, Article>().ReverseMap();
        }
    }

}

//панарин провославная цивилизация в глобальном мире