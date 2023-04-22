using AspNetSamples.Repositories;
using Entities_Context.Entities.UserNews;
using Entities_Context;
using IServices.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UsersRolesRepository : Repository<UsersRoles>, IUsersRolesRepository
    {
        public UsersRolesRepository(UserArticleContext newsAggregatorContext)
            : base(newsAggregatorContext)
        {
        }
    }
}
