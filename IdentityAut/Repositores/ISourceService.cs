using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities_Context.Entities.UserNews;

namespace IServices
{
    public interface ISourceService
    {
        public Task<String> GetServiceNameByIdAsync( Int32 Id);
    }
}
