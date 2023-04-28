using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetSamples.Abstractions.Data.Repositories;

namespace IServices.Repositories
{

    public interface IArticleRepository:IRepository<Article>
    {
        public Task<List<Article>> GetArticlesByPageAsync(Int32 page, Int32 pageSize);
        public Task<List<Article>> GetArticlesByTagByPageAsync(Int32 page, Int32 pageSize, Int32 tag);
        public Task<Int32> GetArticlesWithTagCountAsync(Int32 tagId);
        public Task<List<Article>> GetArticlesBySearchRequestByPageAsync(Int32 page, Int32 pageSize,
            String searchLineRequest);
    }
}
