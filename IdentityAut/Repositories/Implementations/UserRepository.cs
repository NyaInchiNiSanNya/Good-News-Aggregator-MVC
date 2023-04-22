using AspNetSamples.Repositories;
using Entities_Context.Entities.UserNews;
using Entities_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IServices.Repositories;

namespace Repositories.Implementations
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(UserArticleContext newsAggregatorContext)
            : base(newsAggregatorContext)
        {
        }

    }
}
