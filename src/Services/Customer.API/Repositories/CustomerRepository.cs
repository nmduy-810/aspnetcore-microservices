using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryBaseAsync<CatalogCustomer, int, CustomerContext>, ICustomerRepository
{
    public CustomerRepository(CustomerContext dbContext, 
        IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
    {
    }

    public Task<CatalogCustomer?> GetCustomerByUserNameAsync(string username) =>
         FindByCondition(x => x.UserName.Equals(username)).SingleOrDefaultAsync();

    public async Task<IEnumerable<CatalogCustomer>> GetCustomersAsync() => await FindAll().ToListAsync();
}