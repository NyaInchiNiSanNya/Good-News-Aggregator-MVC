using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Core.DTOs.Article;
using Entities_Context;
using Entities_Context.Entities.UserNews;
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

        private PictureBase64EncoderDecoder decoder;


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

            decoder = new PictureBase64EncoderDecoder();

            if (decoder is null)
            {
                throw new ArgumentNullException(nameof(decoder));
            }
        }

        public async Task<List<AutoCompleteDataDto>> GetArticlesNamesByPartNameAsync(String Request)
        {
            var articles = await _unitOfWork.Articles
                .GetAsQueryable()
                .AsNoTracking()
                .Where(article => article.Title.Contains(Request))
                .Select(article => _Mapper.Map<AutoCompleteDataDto>(article))
                .ToListAsync();

            return articles;
        }

        public async Task<List<ArticleDTO>> GetArticlesByPartNameAsync(Int32 page, Int32 pageSize, String searchLineRequest)
        {
            List<ArticleDTO> ArticleList = new List<ArticleDTO>();

            if (String.IsNullOrEmpty(searchLineRequest))
            {

                return await GetShortArticlesWithSourceByPageAsync(page, pageSize);

                //лог
            }
            else
            {
                ArticleList = (await _unitOfWork
                .Articles
                    .GetArticlesBySearchRequestByPageAsync(page, pageSize, searchLineRequest))
                    .Select(article => new ArticleDTO
                    {
                        Id = article.Id,
                        Title = article.Title,
                        PositiveRate = article.PositiveRate,
                        URL = article.URL,
                        ShortDescription = article.ShortDescription,
                        DateTime = article.DateTime,
                        SourceId = article.SourceId,
                        ArticlePicture = decoder.PictureDecoder(article.ArticlePicture)
                    }).ToList();

            }

            if (ArticleList is not null)
            {
                foreach (var article in ArticleList)
                {
                    article.Source = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }

                return ArticleList;
            }

            return null;
        }

        public async Task<List<ArticleDTO>> GetArticlesWithSourceByTagByPageAsync(Int32 page, Int32 pageSize, String tagName)
        {
            var tag = await _unitOfWork.Tag.FindBy(x => x.Name == tagName).FirstOrDefaultAsync();

            List<ArticleDTO> ArticleList = new List<ArticleDTO>();

            if (tag is null)
            {

                ArticleList = (await _unitOfWork
                         .Articles
                         .GetArticlesByPageAsync(page, pageSize))
                     .Select(article => new ArticleDTO
                     {
                         Id = article.Id,
                         Title = article.Title,
                         PositiveRate = article.PositiveRate,
                         URL = article.URL,
                         ShortDescription = article.ShortDescription,
                         DateTime = article.DateTime,
                         SourceId = article.SourceId,
                         ArticlePicture = decoder.PictureDecoder(article.ArticlePicture)
                     }).ToList();

                //лог
            }
            else
            {
                ArticleList = (await _unitOfWork
                    .Articles
                    .GetArticlesByTagByPageAsync(page, pageSize, tag.Id))
                    .Select(article => new ArticleDTO
                    {
                        Id = article.Id,
                        Title = article.Title,
                        PositiveRate = article.PositiveRate,
                        URL = article.URL,
                        ShortDescription = article.ShortDescription,
                        DateTime = article.DateTime,
                        SourceId = article.SourceId,
                        ArticlePicture = decoder.PictureDecoder(article.ArticlePicture)
                    }).ToList();

            }

            if (ArticleList is not null)
            {
                foreach (var article in ArticleList)
                {
                    article.Source = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }

                return ArticleList;
            }

            return null;

        }
        public async Task<List<ArticleDTO>> GetShortArticlesWithSourceByPageAsync(Int32 page, Int32 pageSize)
        {

            List<ArticleDTO> ArticleList = (await _unitOfWork
                    .Articles
                    .GetArticlesByPageAsync(page, pageSize))
                .Select(article => new ArticleDTO
                {
                    Id = article.Id,
                    Title = article.Title,
                    PositiveRate = article.PositiveRate,
                    ShortDescription = article.ShortDescription,
                    URL = article.URL,
                    DateTime = article.DateTime,
                    SourceId = article.SourceId,
                    ArticlePicture = decoder.PictureDecoder(article.ArticlePicture)
                }).ToList();

            if (ArticleList is not null)
            {
                foreach (var article in ArticleList)
                {
                    article.Source = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }
                return ArticleList;
            }

            return null;
        }

        public async Task DeleteArticleById(Int32 articleId)
        {
            await _unitOfWork.Articles.Remove(articleId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Int32> GetTotalArticleCountAsync()
        {
            var count = await _unitOfWork.Articles.CountAsync();
            return count;
        }

        public async Task<Int32> GetArticleCountWithPartNameAsync(String searchLineRequest)
        {
            if (searchLineRequest is not null)
            {
                var count = await _unitOfWork.Articles
                    .GetAsQueryable()
                    .Where(x=>x.Title.Contains(searchLineRequest))
                    .CountAsync();
                
                return count;
            }
            return 0;
        }

        public async Task<Int32> GetArticleCountWithTagAsync(String tagName)
        {
            var tag = await _unitOfWork.Tag.FindBy(x => x.Name == tagName).FirstOrDefaultAsync();

            if (tag is not null)
            {
                var count = await _unitOfWork.Articles.GetArticlesWithTagCountAsync(tag.Id);
                return count;
            }
            return 0;
            
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
