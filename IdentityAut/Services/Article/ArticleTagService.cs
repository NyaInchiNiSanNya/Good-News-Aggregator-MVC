using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using Entities_Context.Entities.UserNews;
using IServices.Services;
using Microsoft.EntityFrameworkCore;

namespace Services.Article
{
    public class ArticleTagService:IArticleTagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleTagService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }
            _unitOfWork = unitOfWork;
        }
        public async Task AddTagsEachArticleAsync(Int32 ArticleId, List<String> tags)
        {
            if (ArticleId!=0 && !(tags is null))
            {
                foreach (var tag in tags)
                {
                    
                    var TagId = await _unitOfWork.Tag.GetAsQueryable()
                        .Where(x => x.Name.Equals(tag))
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync();

                    if (TagId != 0)
                    {
                        var ArticleTag = new ArticleTag()
                        {
                            ArticleId = ArticleId,
                            TagId = TagId
                        };
                        await _unitOfWork.ArticlesTags.AddAsync(ArticleTag);
                        
                    }
                }
            }



        }
    }
}
