using Entities_Context.Entities.UserNews;
using IServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{

    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(UserArticleContext newsAggregatorContext)
            : base(newsAggregatorContext)
        {
        }

    }
}
