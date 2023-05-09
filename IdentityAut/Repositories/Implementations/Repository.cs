using System.Collections.Generic;
using System.Linq.Expressions;
using AspNetSamples.Abstractions.Data.Repositories;
using AspNetSamples.Core;
using Core.DTOs;
using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetSamples.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IBaseEntity
{
    protected readonly UserArticleContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(UserArticleContext userArticleContext)
    {
        Db = userArticleContext;
        DbSet = Db.Set<TEntity>();
    }


    public virtual void Dispose()
    {
        Db.Dispose();
        GC.SuppressFinalize(this);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Int32 id)
    {
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var result = DbSet.Where(predicate);
        if (includes.Any())
        {
            result = includes
                .Aggregate(result,
                    (current, include)
                        => current.Include(include));
        }
        return result;
    }

    public virtual IQueryable<TEntity> GetAsQueryable()
    {
        return DbSet;
    }

    public virtual async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
    {
        return await DbSet.AddAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public virtual async Task PatchAsync(Int32 id, List<PatchDto> patchDtos)
    {
        var entity =
            await DbSet.FirstOrDefaultAsync(ent => ent.Id == id);

        var nameValuePairProperties = patchDtos.ToDictionary
        (k => k.PropertyName,
            v => v.PropertyValue);

        var dbEntityEntry = Db.Entry(entity);
        dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
        dbEntityEntry.State = EntityState.Modified;
    }

    public virtual async Task Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual async Task Remove(Int32 id)
    {
        var entity =
            await DbSet.FirstOrDefaultAsync(ent => ent.Id == id);
        DbSet.Remove(entity);
    }

    public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public virtual async Task<Int32> CountAsync()
    {
        return await DbSet.CountAsync();
    }
}