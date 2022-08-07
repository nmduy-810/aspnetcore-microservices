using Microsoft.EntityFrameworkCore;

namespace Contracts.Common.Interfaces;

public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
{
    // Management commit data to database
    Task<int> CommitAsync();
}