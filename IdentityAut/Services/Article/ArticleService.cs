using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Article;
using Entities_Context;
using IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Services.Article
{
     public class ArticleService : IArticleService
    {
        private readonly UserArticleContext _articleContext;

        private readonly ISourceService _sourceService;


        public ArticleService(UserArticleContext articleContext, ISourceService sourceService)
        {
            if (articleContext is null)
            {
                throw new ArgumentNullException(nameof(articleContext));
            }

            _articleContext = articleContext;

            if (sourceService is null)
            {
                throw new ArgumentNullException(nameof(SourceService));
            }

            _sourceService = sourceService;

        }
        public async Task<List<ArticleDTO>> GetShortArticlesWithSource(Int32 page, Int32 pageSize)
        {
            List<ArticleDTO> ArticleList = await _articleContext.Articles
                .AsNoTracking()
                .OrderBy(article => article.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(article => new ArticleDTO
                {
                    Id = article.Id,
                    ShortDescription = article.ShortDescription,
                    Title = article.Title,
                    FullText = article.FullText,
                    PositiveRate = article.PositiveRate,
                    URL = article.URL,
                    DateTime = article.DateTime,
                    SourceId = article.SourceId,
                    ArticlePicture=Convert.FromBase64String(article.ArticlePicture)
                }).ToListAsync();
            
            
            foreach (var article in ArticleList)
            {
                article.Source = await _sourceService.GetServiceNameByIdAsync(article.SourceId);
            }
            
            return ArticleList;
        }

        public Int32 GetTotalArticleCount()
        {
            return  _articleContext.Articles.AsNoTracking().Count();
        }
    }
}
