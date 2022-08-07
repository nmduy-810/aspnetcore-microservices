using System.Linq.Expressions;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contracts.Common.Interfaces;

/// <summary>
/// Query database (Get)
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TK"></typeparam>
/// <typeparam name="TContext"></typeparam>
public interface IRepositoryQueryBase<T, TK, TContext> 
    where T : EntityBase<TK> 
    where TContext : DbContext
{
    IQueryable<T> FindAll(bool trackChanges = false);
    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object[]>>[] includeProperties);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);
    Task<T?> GetByIdAsync(TK id);
    Task<T?> GetByIdAsync(TK id, params Expression<Func<T, object>>[] includeProperties);
}

/// <summary>
/// Action database (Create/Update/Delete)
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TK"></typeparam>
/// <typeparam name="TContext"></typeparam>
public interface IRepositoryBaseAsync<T, TK, TContext> : IRepositoryQueryBase<T, TK, TContext>
    where T : EntityBase<TK> 
    where TContext : DbContext
{
    Task<TK> CreateAsync(T entity);
    Task<IList<TK>> CreateListAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateListAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteListAsync(IEnumerable<T> entities);
    Task<int> SaveChangesAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    Task RollbackTransactionAsync();

}
