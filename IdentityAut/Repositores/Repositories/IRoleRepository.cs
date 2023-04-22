using AspNetSamples.Abstractions.Data.Repositories;
using Entities_Context.Entities.UserNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Repositories
{
    public interface IRoleRepository : IRepository<UserRole>
    {
    }
}
