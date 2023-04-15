using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices;
using Microsoft.EntityFrameworkCore;

namespace Services.Article
{
    public class SourceService:ISourceService
    {
        private readonly UserArticleContext _articleContext;

        public SourceService(UserArticleContext articleContext)
        {
            if (articleContext is null)
            {
                throw new ArgumentNullException(nameof(articleContext));
            }

            _articleContext = articleContext;
        }

        public async Task<String> GetServiceNameByIdAsync(Int32 Id)
        {
            return await _articleContext.Sources
                .AsNoTracking()
                .Where(source => source.Id == Id)
                .Select(Source=>Source.Name)
                .FirstOrDefaultAsync();
            
        }
    }
}
