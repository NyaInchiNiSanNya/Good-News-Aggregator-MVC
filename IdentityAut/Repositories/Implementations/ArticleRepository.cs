
using Entities_Context;
using Entities_Context.Entities.UserNews;
using IServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Repositories;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    public ArticleRepository(UserArticleContext newsAggregatorContext)
        : base(newsAggregatorContext)
    {
    }

    public async Task<List<Article>> GetArticlesByPageAsync(Int32 page, Int32 pageSize)
    {
        var articles = await DbSet
            .Include(article => article.Source)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return articles;
    }
}