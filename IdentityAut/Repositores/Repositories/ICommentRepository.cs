using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Entities_Context.Entities.UserNews;

namespace IServices.Repositories
{
    public interface ICommentRepository:IRepository<Comment>
    {
    }
}
