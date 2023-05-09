using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetSamples.Abstractions.Data.Repositories;

namespace IServices.Repositories
{

    public interface IArticleTagRepository : IRepository<ArticleTag>
    {
    }
}