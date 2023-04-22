using AspNetSamples.Repositories;
using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetSamples.Abstractions.Data.Repositories;
using IServices.Repositories;
using Entities_Context;

namespace Repositories.Implementations
{
    public class RoleRepository: Repository<UserRole>, IRoleRepository
    {
        public RoleRepository(UserArticleContext newsAggregatorContext)
            : base(newsAggregatorContext)
        {
        }
    }
}
