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
    }
}
