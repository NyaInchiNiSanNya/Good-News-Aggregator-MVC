using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Abstract;
using AutoMapper;
using Core.DTOs.Article;
using IServices;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using IServices.Services;
using Microsoft.Extensions.Configuration;
using Services.Article.ArticleRate;
using Services.Article.WebParsers;
using Serilog;

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
                return await GetShortArticlesWithSourceByPageAsync(page, pageSize, 0);
            }
            else
            {
                var Articles = await _unitOfWork
                        .Articles
                        .GetArticlesBySearchRequestByPageAsync(page, pageSize, searchLineRequest);

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

        public async Task<List<ArticleDTO>> GetArticlesWithSourceByTagByPageAsync(Int32 page, Int32 pageSize,
            String tagName, Int32 userRateFilter)
        {
            var tag = await _unitOfWork.Tag.FindBy(x => x.Name == tagName).FirstOrDefaultAsync();

            List<ArticleDTO> ArticleList = new List<ArticleDTO>();

            if (tag is null)
            {

                var Articles = await _unitOfWork
                    .Articles
                    .GetArticlesByPageAsync(page, pageSize, userRateFilter);

                ArticleList = _Mapper.Map<List<ArticleDTO>>(Articles);
            }
            else
            {
                var Articles = await _unitOfWork
                    .Articles
                    .GetArticlesByTagByPageAsync(page, pageSize, tag.Id, userRateFilter);

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

        public async Task<List<ArticleDTO>> GetShortArticlesWithSourceByPageAsync(Int32 page, Int32 pageSize, Int32 userRateFilter)
        {
            List<ArticleDTO> ArticleList = new List<ArticleDTO>();

            var Articles = await _unitOfWork
                .Articles
                .GetArticlesByPageAsync(page, pageSize, userRateFilter);

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

        public async Task<Int32> GetArticleCount(String tagName = "", Int32 userRateFilter = 0, String searchLineRequest = "")
        {
            Int32 count = 0;
            if (!String.IsNullOrEmpty(searchLineRequest))
            {
                count = await _unitOfWork.Articles
                    .GetAsQueryable()
                    .Where(x => x.Title.Contains(searchLineRequest))
                    .CountAsync();

                return count;
            }

            if (!String.IsNullOrEmpty(tagName) && String.IsNullOrEmpty(searchLineRequest))
            {
                var tagSearch = await _unitOfWork.Tag.FindBy(x => x.Name == tagName).FirstOrDefaultAsync();

                if (tagSearch is not null)
                {
                    count = await _unitOfWork.Articles.GetAsQueryable()
                        .Include(article => article.Source)
                        .Include(x => x.Tags)
                        .Where(article => article.Tags.Any(tag => tag.TagId == tagSearch.Id)).CountAsync();
                    return count;
                }

                return 0;
            }

            count = await _unitOfWork.Articles.GetAsQueryable()
                .Where(article => article.PositiveRate >= userRateFilter).CountAsync();

            return count;

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

                    ArticleSentimentAnalyzer sentimentAnalyzer = new ArticleSentimentAnalyzer(_сonfiguration);

                    fullArticlesDTOsFromRss =
                        await sentimentAnalyzer.GetArticlesWithSentimentScore(fullContentArticles);

                    await AddArticlesAsync(fullArticlesDTOsFromRss);

                    foreach (var FullArticle in fullArticlesDTOsFromRss)
                    {
                        var articleId = await _unitOfWork.Articles.GetAsQueryable()
                            .Where(x => x.HashUrlId.Equals(FullArticle.HashUrlId)).Select(x => x.Id).FirstOrDefaultAsync();

                        await _articleTagService.AddTagsEachArticleAsync(articleId, FullArticle.ArticleTags);
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else
            {
                Log.Warning("Aggregation failed.Sources not found");
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
                        var Parser =
                            Activator.CreateInstance(Type.GetType(sourceConfigValue), dto.ArticleSourceUrl) as
                                AbstractParser;

                        dto.ArticlePicture = Parser.GetPictureReference();
                        dto.ShortDescription = Parser.GetShortDescription();
                        dto.FullText = Parser.GetFullTextDescription();
                        dto.ArticleSourceUrl = Parser.GetArticleSourceReference(dto.ArticleSourceUrl);
                        dto.ArticleTags = Parser.GetArticleTagsFromRss(dto.ArticleTags);

                    }
                    catch (Exception e)
                    {
                        Log.Warning("Unsuccessful attempt to aggregate news: {0}", dto.ArticleSourceUrl);
                    }


                    concBag.Add(dto);
                }
                else
                {
                    throw new ArgumentException("Source was not found in the configuration file");
                }
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
                                Log.Warning("Unsuccessful attempt to aggregate news tags");
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
