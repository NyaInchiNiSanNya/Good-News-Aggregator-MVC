using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.Article
{
    public class ArticleTagService : IArticleTagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleTagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        
        
        public async Task AddTagsEachArticleAsync(Int32 articleId, List<String> tags)
        {
            if (articleId < 0)
            {

                throw new ArgumentException("Invalid article parameters");
            }

            foreach (var tag in tags)
            {

                var tagId = await _unitOfWork.Tag.GetAsQueryable()
                    .Where(x => x.Name.Equals(tag))
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (tagId != 0)
                {
                    var articleTag = new ArticleTag()
                    {
                        ArticleId = articleId,
                        TagId = tagId
                    };
                    await _unitOfWork.ArticlesTags.AddAsync(articleTag);

                }
            }

        }
    }
}
