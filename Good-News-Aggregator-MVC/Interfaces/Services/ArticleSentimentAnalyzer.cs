using Core.DTOs.Article;

namespace IServices.Services
{
    public interface IArticleSentimentAnalyzer
    {

        public Task<List<FullArticleDto>> GetArticlesWithSentimentScore(List<FullArticleDto> fullContentArticles);
    }
}
