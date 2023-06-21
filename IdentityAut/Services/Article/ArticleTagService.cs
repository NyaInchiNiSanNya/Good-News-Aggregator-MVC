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
                Log.Error("Invalid article tags parameters: id={0}, {1} articles", 
                    articleId, tags.Count);
                
                throw new ArgumentException();
            }

            foreach (var tag in tags)
            {

                var TagId = await _unitOfWork.Tag.GetAsQueryable()
                    .Where(x => x.Name.Equals(tag))
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (TagId != 0)
                {
                    var articleTag = new ArticleTag()
                    {
                        ArticleId = articleId,
                        TagId = TagId
                    };
                    await _unitOfWork.ArticlesTags.AddAsync(articleTag);

                }
            }




        }
    }
}
