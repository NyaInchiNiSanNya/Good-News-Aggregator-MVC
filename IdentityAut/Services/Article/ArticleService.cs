using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Abstract;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Core.DTOs.Article;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using IServices.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Services.Article.WebParsers;

namespace Services.Article
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;
        private readonly ISourceService _sourceService;
        private readonly IConfiguration _сonfiguration;
        private readonly IArticleTagService _articleTagService;


        public ArticleService(IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ISourceService sourceService, 
            IConfiguration сonfiguration,
            IArticleTagService articleTagService)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;

            if (articleTagService is null)
            {
                throw new ArgumentNullException(nameof(articleTagService));
            }

            _articleTagService = articleTagService;

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

            if (сonfiguration is null)
            {
                throw new ArgumentNullException(nameof(сonfiguration));
            }
            _сonfiguration = сonfiguration;

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

        public async Task<List<ArticleDTO>> GetArticlesByPartNameAsync(Int32 page, Int32 pageSize,
            String searchLineRequest)
        {
            List<ArticleDTO> ArticleList = new List<ArticleDTO>();

            if (String.IsNullOrEmpty(searchLineRequest))
            {

                return await GetShortArticlesWithSourceByPageAsync(page, pageSize);

                //лог
            }
            else
            {
                var Articles = await _unitOfWork
                        .Articles
                        .GetArticlesBySearchRequestByPageAsync(page, pageSize, searchLineRequest);
               
                ArticleList= _Mapper.Map<List<ArticleDTO>>(Articles);

            }

            if (ArticleList is not null)
            {
                foreach (var article in ArticleList)
                {
                    article.SourceName = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }

                return ArticleList;
            }

            return null;
        }

        public async Task<List<ArticleDTO>> GetArticlesWithSourceByTagByPageAsync(Int32 page, Int32 pageSize,
            String tagName)
        {
            var tag = await _unitOfWork.Tag.FindBy(x => x.Name == tagName).FirstOrDefaultAsync();

            List<ArticleDTO> ArticleList = new List<ArticleDTO>();

            if (tag is null)
            {

                var Articles = await _unitOfWork
                    .Articles
                    .GetArticlesByPageAsync(page, pageSize);
                
                ArticleList = _Mapper.Map<List<ArticleDTO>>(Articles);
                //лог
            }
            else
            {
                var Articles = await _unitOfWork
                    .Articles
                    .GetArticlesByTagByPageAsync(page, pageSize, tag.Id);

                ArticleList = _Mapper.Map<List<ArticleDTO>>(Articles);
            }

            if (ArticleList is not null)
            {
                foreach (var article in ArticleList)
                {
                    article.SourceName = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }

                return ArticleList;
            }

            return null;

        }

        public async Task<List<ArticleDTO>> GetShortArticlesWithSourceByPageAsync(Int32 page, Int32 pageSize)
        {

            List<ArticleDTO> ArticleList = new List<ArticleDTO>();
            
            var Articles = await _unitOfWork
                .Articles
                .GetArticlesByPageAsync(page, pageSize);

            ArticleList = _Mapper.Map<List<ArticleDTO>>(Articles);
            
            if (ArticleList is not null)
            {
                foreach (var article in ArticleList)
                {
                    article.SourceName = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
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
                    .Where(x => x.Title.Contains(searchLineRequest))
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
                fullArticle.SourceName = await _sourceService.GetSourceNameByIdAsync(fullArticle.SourceId);

                return fullArticle;
            }

            return null;
        }

        public async Task AggregateArticlesAsync()
        {
            List<SourceDTO>? sources = await _sourceService.GetAllSourcesDTOAsync();

            if (sources is not null)
            {
                foreach (var source in sources)
                {
                    var fullArticlesDTOsFromRss =
                        await AggregateArticlesDataFromRssSourceAsync(source, CancellationToken.None);

                    var fullContentArticles = await GetFullContentArticlesAsync(fullArticlesDTOsFromRss);

                    await AddArticlesAsync(fullContentArticles);
                    
                    foreach (var FullArticle in fullContentArticles)
                    {
                        var articleId = await _unitOfWork.Articles.GetAsQueryable()
                            .Where(x => x.HashUrlId.Equals(FullArticle.HashUrlId)).Select(x=>x.Id).FirstOrDefaultAsync();
                        
                        await _articleTagService.AddTagsEachArticleAsync(articleId, FullArticle.ArticleTags);
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
            }

        }

        private async Task<List<FullArticleDTO>> GetFullContentArticlesAsync(IEnumerable<FullArticleDTO> articleDTOsFromRss)
        {
            var concBag = new ConcurrentBag<FullArticleDTO>();


            await Parallel.ForEachAsync(articleDTOsFromRss, async (dto, token) =>
            {
                var sourceConfigValue = _сonfiguration["ResourceHandlers:" + dto.SourceName];

                if (!String.IsNullOrEmpty(sourceConfigValue))
                {
                    try
                    {
                        var Parser = Activator.CreateInstance(Type.GetType(sourceConfigValue), dto.ArticleSourceUrl) as AbstractParser;

                        dto.ArticlePicture = Parser.GetPictureReference();
                        dto.ShortDescription = Parser.GetShortDescription();
                        dto.FullText = Parser.GetFullTextDescription();
                        dto.ArticleSourceUrl = Parser.GetArticleSourceReference(dto.ArticleSourceUrl);
                        dto.ArticleTags = Parser.GetArticleTagsFromRss(dto.ArticleTags);

                    }
                    catch (Exception e)
                    {
                        
                    }

                }
                concBag.Add(dto);
            });

            return concBag.ToList();
        }

        private async Task AddArticlesAsync(IEnumerable<FullArticleDTO> articleDTOs)
        {
            var entities = articleDTOs
                .Select(a => _Mapper
                    .Map<Entities_Context.Entities.UserNews.Article>(a))
                .ToList();

            await _unitOfWork.Articles.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
        }


        private async Task<List<FullArticleDTO>> AggregateArticlesDataFromRssSourceAsync(SourceDTO source,
            CancellationToken cancellationToken)
        {
            var articles = new ConcurrentBag<FullArticleDTO>();

            var ArticleHash = await GetContainsArticleIdBySourceAsync(source.Id);

            using (var reader = XmlReader.Create(source.RssFeedUrl))
            {
                var feed = SyndicationFeed.Load(reader);

                await Parallel.ForEachAsync(feed.Items
                        .Where(item => 
                            !ArticleHash.Contains(ComputeSHA256Hash(source.RssFeedUrl + item.Id))).ToArray(), cancellationToken,
                    (item, token) =>
                    {

                        FullArticleDTO articleDto = new FullArticleDTO()
                        {

                            ArticleSourceUrl = item.Id,
                            HashUrlId = ComputeSHA256Hash(source.RssFeedUrl + item.Id),
                            SourceId = source.Id,
                            SourceUrl = source.OriginUrl,
                            SourceName = source.Name,
                            ArticleTags = new List<String>(),
                            Title = item.Title.Text,
                            DateTime = item.PublishDate.DateTime
                        };
                        foreach (var category in item.Categories)
                        {
                            try
                            {
                                articleDto.ArticleTags.Add(category.Name);
                            }
                            catch (NullReferenceException)
                            {
                                
                            }
                            
                        }

                        articles.Add(articleDto);
                        return ValueTask.CompletedTask;
                    });

                reader.Close();
                return articles.ToList();

            }
        }
        public string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        
        private async Task<List<String>> GetContainsArticleIdBySourceAsync(Int32 sourceId)
        {
            var articlesURL = await _unitOfWork.Articles
                .GetAsQueryable()
                .Where(x => x.SourceId == sourceId)
                .Select(x => x.HashUrlId)
                .ToListAsync();

            if (articlesURL is null)
            {
                return null;
            }

            return articlesURL;
        }
    }
}
