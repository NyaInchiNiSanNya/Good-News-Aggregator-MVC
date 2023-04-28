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
        public Task<List<ArticleDTO>> GetShortArticlesWithSourceByPageAsync(Int32 page,Int32 pageSize);
        public Task<List<ArticleDTO>> GetArticlesWithSourceByTagByPageAsync(Int32 page, Int32 pageSize, String tag);
        public Task<List<AutoCompleteDataDto>> GetArticlesNamesByPartNameAsync(String Request);
        public Task<List<ArticleDTO>> GetArticlesByPartNameAsync(Int32 page, Int32 pageSize, String searchLineRequest);
        public Task<Int32> GetArticleCountWithTagAsync(String tag);
        public Task<Int32> GetArticleCountWithPartNameAsync(String searchLineRequest);
        public Task DeleteArticleById(Int32 id);
        public Task<Int32> GetTotalArticleCountAsync();
        public Task<FullArticleDTO> GetFullArticleByIdAsync(Int32 id);
    }
}
