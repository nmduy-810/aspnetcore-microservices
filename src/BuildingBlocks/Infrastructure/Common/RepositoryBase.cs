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

    public void Create(T entity) => _dbContext.Set<T>().Add(entity);

    public async Task<TK> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public IList<TK> CreateList(IEnumerable<T> entities)
    {
        var entityBases = entities.ToList();
        _dbContext.Set<T>().AddRange(entityBases);
       return entityBases.Select(x => x.Id).ToList();
    }

    public async Task<IList<TK>> CreateListAsync(IEnumerable<T> entities)
    {
        var entityBases = entities.ToList();
        await _dbContext.Set<T>().AddRangeAsync(entityBases);
        return entityBases.Select(x => x.Id).ToList();
    }

    public void Update(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged)
            return;
        
        _dbContext.Entry(entity).CurrentValues.SetValues(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged) 
            return;
        
        _dbContext.Entry(entity).CurrentValues.SetValues(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

    public async Task DeleteAsync(T entity) 
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public void DeleteList(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

    public async Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();
}