using Core.DTOs.Article;

namespace IServices.Services
{
    public interface IArticleService
    {
        public Task<List<ShortArticleDto>?> GetShortArticlesWithSourceByPageAsync(Int32 page, 
            Int32 pageSize, Int32 userRateFilter);
        
        public Task<List<ShortArticleDto>?> GetArticlesWithSourceByTagByPageAsync(Int32 page, Int32 pageSize,
            String tag, Int32 userRateFilter);
        
        public Task<List<AutoCompleteDataDto>> GetArticlesNamesByPartNameAsync(String request);
        
        public Task<List<ShortArticleDto>?> GetArticlesByPartNameAsync(Int32 page,
            Int32 pageSize, String searchLineRequest);
        
        public Task<Boolean> DeleteArticleById(Int32 id);
        
        public Task<Int32> GetArticleCount(String tagName = "", Int32 userRateFilter = 0,
            String searchLineRequest = "");
        
        public Task<FullArticleDto?> GetFullArticleByIdAsync(Int32 id);
        
        public Task AggregateArticlesAsync();
    }
}
