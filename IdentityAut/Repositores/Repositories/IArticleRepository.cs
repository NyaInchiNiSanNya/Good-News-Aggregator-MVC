using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Repositories
{

    public interface IArticleRepository:IRepository<Article>
    {
        public Task<List<Article>> GetArticlesByPageAsync(Int32 page, Int32 pageSize, Int32 userRateFilter);
        public Task<List<Article>> GetArticlesByTagByPageAsync(Int32 page, Int32 pageSize, Int32 tag, Int32 userRateFilter);
        public Task<List<Article>> GetArticlesBySearchRequestByPageAsync(Int32 page, Int32 pageSize,
            String searchLineRequest);
    }
}
