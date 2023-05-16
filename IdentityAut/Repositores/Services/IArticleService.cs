using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Core.DTOs.Article;
using Entities_Context.Entities.UserNews;

namespace IServices
{
    public interface IArticleService
    {
        public Task<List<ArticleDTO>> GetShortArticlesWithSourceByPageAsync(Int32 page, 
            Int32 pageSize, Int32 userRateFilter);
        
        public Task<List<ArticleDTO>> GetArticlesWithSourceByTagByPageAsync(Int32 page, Int32 pageSize,
            String tag, Int32 userRateFilter);
        
        public Task<List<AutoCompleteDataDto>> GetArticlesNamesByPartNameAsync(String Request);
        
        public Task<List<ArticleDTO>> GetArticlesByPartNameAsync(Int32 page,
            Int32 pageSize, String searchLineRequest);
        
        public Task DeleteArticleById(Int32 id);
        
        public Task<Int32> GetArticleCount(String tagName = "", Int32 userRateFilter = 0,
            String searchLineRequest = "");
        
        public Task<FullArticleDTO> GetFullArticleByIdAsync(Int32 id);
        
        public Task AggregateArticlesAsync();
    }
}
