using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Article;

namespace IServices
{
    public interface IArticleService
    {
        public Task<List<ArticleDTO>> GetShortArticlesWithSourceByPageAsync(Int32 Page,Int32 PageSize);
        public Task<Int32> GetTotalArticleCountAsync();
        public Task<FullArticleDTO> GetFullArticleByIdAsync(Int32 Id);
    }
}
