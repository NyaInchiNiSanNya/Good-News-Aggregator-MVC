using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.DTOs.Article;
using Entities_Context;
using IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Services.Article
{
     public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;
        private readonly ISourceService _sourceService;


        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, ISourceService sourceService)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;
            
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _Mapper = mapper;

            if (sourceService is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _sourceService = sourceService;
        }
        
        
        public async Task<List<ArticleDTO>> GetShortArticlesWithSourceByPageAsync(Int32 page, Int32 pageSize)
        {

            List<ArticleDTO> ArticleList =(await _unitOfWork
                    .Articles
                    .GetArticlesByPageAsync(page, pageSize))
                .Select(article => new ArticleDTO
                {
                    Id = article.Id,
                    Title = article.Title,
                    PositiveRate = article.PositiveRate,
                    URL = article.URL,
                    DateTime = article.DateTime,
                    SourceId = article.SourceId,
                    ArticlePicture=Convert.FromBase64String(article.ArticlePicture)
                }).ToList();

            if (ArticleList is not null)
            {
                foreach (var article in ArticleList)
                {
                    article.Source = await _sourceService.GetSourceNameByIdAsync(article.Id);
                }
                return ArticleList;
            }

            return null;
        }


        public async Task<Int32> GetTotalArticleCountAsync()
        {
            var count = await _unitOfWork.Articles.CountAsync();
            return count;
        }


        public async Task<FullArticleDTO> GetFullArticleByIdAsync(Int32 Id)
        {
            FullArticleDTO fullArticle = _Mapper.Map<FullArticleDTO>(await _unitOfWork.Articles.GetByIdAsync(Id));

            if (fullArticle is not null)
            {
                fullArticle.Source = await _sourceService.GetSourceNameByIdAsync(fullArticle.SourceId);
                
                return fullArticle;
            }

            return null;
        }
    }
}
