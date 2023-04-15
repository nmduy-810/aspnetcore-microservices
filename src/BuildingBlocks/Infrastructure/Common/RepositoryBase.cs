using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common;

public class RepositoryBase<T, TK, TContext> : RepositoryQueryBase<T, TK, TContext>,
    IRepositoryBase<T, TK, TContext> where T : EntityBase<TK> where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IUnitOfWork<TContext> _unitOfWork;
    
    public RepositoryBase(TContext dbContext, IUnitOfWork<TContext> unitOfWork) : base(dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();

    public async Task EndTransactionAsync()
    { 
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync() => _dbContext.Database.RollbackTransactionAsync();

    public async Task<TK> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    public async Task<IList<TK>> CreateListAsync(IEnumerable<T> entities)
    {
        var entityBases = entities.ToList();
        await _dbContext.Set<T>().AddRangeAsync(entityBases);
        return entityBases.Select(x => x.Id).ToList();
    }

    public Task UpdateAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged) 
            return Task.CompletedTask;
        
        var exist = _dbContext.Set<T>().Find(entity.Id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);

        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<T> entities) => _dbContext.Set<T>().AddRangeAsync(entities);

    public Task DeleteAsync(T entity) 
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
    
    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();
}