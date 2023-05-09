using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Services
{
    public interface IArticleTagService
    {
        public Task AddTagsEachArticleAsync(Int32 Id, List<String> tags);
    }
}
