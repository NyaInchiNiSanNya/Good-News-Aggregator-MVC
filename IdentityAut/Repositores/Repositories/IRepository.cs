using System.Linq.Expressions;
using AspNetSamples.Core;
using Core.DTOs;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetSamples.Abstractions.Data.Repositories;

public interface IRepository<T> : IDisposable
    where T : class, IBaseEntity
{
    public Task<T?> GetByIdAsync(Int32 id);
    public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    public IQueryable<T> GetAsQueryable();

    Task<EntityEntry<T>> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    Task PatchAsync(int id, List<PatchDto> patchDtos);
    Task Update(T entity);

    Task Remove(int id);
    Task RemoveRange(IEnumerable<T> entities);

    Task<int> CountAsync();

}
