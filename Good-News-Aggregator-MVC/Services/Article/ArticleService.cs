using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
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
using Azure;
using Entities_Context.Entities.UserNews;

namespace Services.Article
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISourceService _sourceService;
        private readonly IConfiguration _сonfiguration;
        private readonly IArticleSentimentAnalyzer _articleSentimentAnalyzer;
        private readonly IArticleTagService _articleTagService;


        public ArticleService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ISourceService sourceService,
            IConfiguration сonfiguration,
            IArticleTagService articleTagService,
            IArticleSentimentAnalyzer articleSentimentAnalyzer)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

            _articleTagService = articleTagService ?? throw new ArgumentNullException(nameof(articleTagService));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));

            _сonfiguration = сonfiguration ?? throw new ArgumentNullException(nameof(сonfiguration));

            _articleSentimentAnalyzer = articleSentimentAnalyzer ?? throw new ArgumentNullException(nameof(articleSentimentAnalyzer));
        }


        public async Task<List<AutoCompleteDataDto>> GetArticlesNamesByPartNameAsync(String request)
        {

            var articles = await _unitOfWork.Articles
                .GetAsQueryable()
                .AsNoTracking()
                .Where(article => article.Title.Contains(request))
                .OrderByDescending(x => x.DateTime)
                .Select(article => _mapper.Map<AutoCompleteDataDto>(article))
                .ToListAsync();

            return articles;
        }

        public async Task<List<ShortArticleDto>?> GetArticlesByPartNameAsync(Int32 page, Int32 pageSize,
            String searchLineRequest)
        {

            if (page < 1 || pageSize < 1)
            {
                throw new ArgumentException("Invalid page parameters");
            }

            List<ShortArticleDto>? articleList;

            if (String.IsNullOrEmpty(searchLineRequest))
            {
                Log.Warning("Attempt get articles by search line with empty request");

                return await GetShortArticlesWithSourceByPageAsync(page, pageSize, 0);
            }
            else
            {
                var Articles = await _unitOfWork
                        .Articles
                        .GetArticlesBySearchRequestByPageAsync(page, pageSize, searchLineRequest);

                articleList = _mapper.Map<List<ShortArticleDto>>(Articles);

            }

            if (articleList is not null)
            {
                foreach (var article in articleList)
                {
                    article.SourceName = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }

                return articleList;
            }
            
            Log.Warning("GetArticlesByPartNameAsync return null article list");
            
            return null;
        }

        public async Task<List<ShortArticleDto>?> GetArticlesWithSourceByTagByPageAsync(Int32 page, Int32 pageSize,
            String tagName, Int32 userRateFilter)
        {

            if (page < 1 || pageSize < 1 || userRateFilter < 0 || userRateFilter > 10)
            {
                throw new ArgumentException("Invalid page parameters");
            }

            var tag = await _unitOfWork.Tag
                .FindBy(x => x.Name == tagName)
                .FirstOrDefaultAsync();

            List<ShortArticleDto>? articleList;

            if (tag == null)
            {
                var articles = await _unitOfWork
                    .Articles
                    .GetArticlesByPageAsync(page, pageSize, userRateFilter);

                articleList = _mapper.Map<List<ShortArticleDto>>(articles);
            }
            else
            {

                var articles = await _unitOfWork
                    .Articles
                    .GetArticlesByTagByPageAsync(page, pageSize, tag.Id, userRateFilter);

                articleList = _mapper.Map<List<ShortArticleDto>>(articles);
            }

            if (articleList is not null)
            {
                foreach (var article in articleList)
                {
                    article.SourceName = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }

                return articleList;
            }

            return null;

        }

        public async Task<List<ShortArticleDto>?> GetShortArticlesWithSourceByPageAsync(Int32 page, Int32 pageSize, Int32 userRateFilter)
        {
            if (page < 1 || pageSize < 1 || userRateFilter < 0 || userRateFilter > 10)
            {
                Log.Error("Invalid page parameters");

                throw new ArgumentException();
            }

            List<ShortArticleDto>? articleList;

            var articles = await _unitOfWork
                .Articles
                .GetArticlesByPageAsync(page, pageSize, userRateFilter);

            articleList = _mapper.Map<List<ShortArticleDto>>(articles);

            if (articleList != null)
            {
                foreach (var article in articleList)
                {
                    article.SourceName = await _sourceService.GetSourceNameByIdAsync(article.SourceId);
                }

                return articleList;
            }

            return null;
        }

        public async Task<Boolean> DeleteArticleById(Int32 articleId)
        {
            if (articleId < 1)
            {
                Log.Error("Invalid page parameters");
                throw new ArgumentException(nameof(articleId));
            }

            Entities_Context.Entities.UserNews.Article? article = await _unitOfWork.Articles.GetByIdAsync(articleId);

            if (article != null)
            {
                await _unitOfWork.Articles.Remove(articleId);
                await _unitOfWork.SaveChangesAsync();

                Log.Information("Article {0} was deleted", articleId);
                
                return true;
            }
            else
            {
                Log.Warning("attempt to delete a non-existent article", articleId);
                return false;
            }
        }

        public async Task<Int32> GetArticleCount(String tagName = "", Int32 userRateFilter = 0, String searchLineRequest = "")
        {

            if (userRateFilter < 0 || userRateFilter > 10)
            {
                Log.Error("Invalid page parameters");
                throw new ArgumentException();
            }

            Int32 count = 0;

            if (!String.IsNullOrEmpty(searchLineRequest))
            {
                count = await _unitOfWork.Articles
                    .GetAsQueryable()
                    .Where(x => x.Title.Contains(searchLineRequest))
                    .CountAsync();

                return count;
            }

            if (!String.IsNullOrEmpty(tagName))
            {
                var tagSearch = await _unitOfWork.Tag.FindBy(x => x.Name == tagName).FirstOrDefaultAsync();

                if (tagSearch != null)
                {
                    count = await _unitOfWork.Articles.GetAsQueryable()
                        .Include(article => article.Source)
                        .Include(x => x.Tags)
                        .Where(article => article.Tags.Any(tag => tag.TagId == tagSearch.Id))
                        .Where(article => article.PositiveRate >= userRateFilter)
                        .CountAsync();
                    return count;
                }

                return 0;
            }

            count = await _unitOfWork.Articles.GetAsQueryable()
                .Where(article => article.PositiveRate >= userRateFilter).CountAsync();

            return count;

        }

        public async Task<FullArticleDto?> GetFullArticleByIdAsync(Int32 id)
        {
            FullArticleDto? fullArticle = _mapper.Map<FullArticleDto>(await _unitOfWork.Articles.GetByIdAsync(id));

            if (fullArticle is not null)
            {
                fullArticle.SourceName = await _sourceService.GetSourceNameByIdAsync(fullArticle.SourceId);
                return fullArticle;
            }

            Log.Warning("attempt to get a non-existent article");

            return null;
        }


        public async Task AggregateArticlesAsync()
        {

            Log.Information("the aggregation of articles has begun");

            List<SourceDto>? sources = await _sourceService.GetAllSourcesDtoAsync();

            if (sources.Count > 0)
            {
                foreach (var source in sources)
                {
                    var fullArticlesDtOsFromRss =
                        await AggregateArticlesDataFromRssSourceAsync(source, CancellationToken.None);

                    var fullContentArticles = await GetFullContentArticlesAsync(fullArticlesDtOsFromRss);

                    fullArticlesDtOsFromRss =
                        await _articleSentimentAnalyzer.GetArticlesWithSentimentScore(fullContentArticles);

                    await AddArticlesAsync(fullArticlesDtOsFromRss);

                    foreach (var fullArticle in fullArticlesDtOsFromRss)
                    {
                        var articleId = await _unitOfWork.Articles.GetAsQueryable()
                            .Where(x => x.HashUrlId.Equals(fullArticle.HashUrlId)).Select(x => x.Id).FirstOrDefaultAsync();

                        await _articleTagService.AddTagsEachArticleAsync(articleId, fullArticle.ArticleTags);
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else
            {
                throw new NullReferenceException("no sources found");
            }

            Log.Information("the aggregation of articles has end");
        }

        private async Task<List<FullArticleDto>> GetFullContentArticlesAsync(IEnumerable<FullArticleDto>? articleDtOsFromRss)
        {
            Log.Information("article parsing has begun");

            var concBag = new ConcurrentBag<FullArticleDto>();

            await Parallel.ForEachAsync(articleDtOsFromRss, (dto, token) =>
            {

                var sourceConfigValue = _сonfiguration["ResourceHandlers:" + dto.SourceName];

                if (!String.IsNullOrEmpty(sourceConfigValue))
                {
                    try
                    {
                        var parser =
                            Activator.CreateInstance(Type.GetType(sourceConfigValue)
                                                     ?? throw new InvalidOperationException("class not found"),
                                    dto.ArticleSourceUrl) as
                                AbstractParser;

                        dto.ArticlePicture = parser.GetPictureReference();
                        dto.ShortDescription = parser.GetShortDescription();
                        dto.FullText = parser.GetFullTextDescription();
                        dto.ArticleSourceUrl = parser.GetArticleSourceReference(dto.ArticleSourceUrl);
                        dto.ArticleTags = parser.GetArticleTagsFromRss(dto.ArticleTags);

                    }
                    catch (Exception e)
                    {
                        Log.Warning("Unsuccessful attempt to aggregate article: {0}, {1}",
                            dto.ArticleSourceUrl, e.Message);
                    }


                    concBag.Add(dto);
                }
                else
                {
                    throw new ArgumentException("rss aggregation failed. The source was not found in the configuration file");
                }

                return ValueTask.CompletedTask;
            });


            Log.Information("article parsing has end. {0} articles were received",
               concBag.Count);

            return concBag.ToList();

        }

        private async Task AddArticlesAsync(IEnumerable<FullArticleDto> articleDtOs)
        {
            var entities = articleDtOs
                .Select(a => _mapper
                    .Map<Entities_Context.Entities.UserNews.Article>(a))
                .ToList();

            await _unitOfWork.Articles.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();

            Log.Information("{0} news has been added", articleDtOs.Count());
        }


        private async Task<List<FullArticleDto>> AggregateArticlesDataFromRssSourceAsync(SourceDto source,
            CancellationToken cancellationToken)
        {
            Log.Information("the aggregation of articles from rss has begun");

            var articles = new ConcurrentBag<FullArticleDto>();

            var articleHash = await GetArticlesIdBySourceIdAsync(source.Id);

            using (var reader = XmlReader.Create(source.RssFeedUrl))
            {
                var feed = SyndicationFeed.Load(reader);

                await Parallel.ForEachAsync(feed.Items
                        .Where(item =>
                            !articleHash.Contains(ComputeSha256Hash(source.RssFeedUrl + item.Id))).ToArray(), cancellationToken,
                    (item, token) =>
                    {

                        FullArticleDto articleDto = new FullArticleDto()
                        {

                            ArticleSourceUrl = item.Id,
                            HashUrlId = ComputeSha256Hash(source.RssFeedUrl + item.Id),
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
                                Log.Warning("Unsuccessful attempt to aggregate news tags: {0}", articleDto.HashUrlId);
                            }

                        }

                        articles.Add(articleDto);
                        return ValueTask.CompletedTask;
                    });

                reader.Close();
                Log.Information("the aggregation of articles from rss has end. Received {0} news", articles.Count);

                return articles.ToList();

            }
        }
        public string ComputeSha256Hash(String input)
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

        private async Task<List<String>?> GetArticlesIdBySourceIdAsync(Int32 sourceId)
        {
            if (sourceId < 1)
            {
                Log.Error("Invalid source id");
                throw new ArgumentException(nameof(sourceId));
            }

            var articles = await _unitOfWork.Articles
                .GetAsQueryable()
                .AsNoTracking()
                .Where(x => x.SourceId == sourceId)
                .Select(x => x.HashUrlId)
                .ToListAsync();

            return articles;
        }
    }
}
