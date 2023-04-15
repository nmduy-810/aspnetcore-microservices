using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryBaseAsync<CatalogCustomer, int, CustomerContext>, ICustomerRepository
{
    public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
    {
    }

    public Task<CatalogCustomer> GetCustomerByUserNameAsync(string userName) =>
         FindByCondition(x => x.UserName.Equals(userName)).SingleAsync();

    public async Task<IEnumerable<CatalogCustomer>> GetCustomerAsync() => await FindAll().ToListAsync();

    public async Task<CatalogCustomer?> GetCustomerById(int id) => await GetByIdAsync(id);

    public async Task<int> CreateCustomer(CatalogCustomer customer)
    {
        await CreateAsync(customer); 
        return await SaveChangesAsync();
    }

    public async Task<int> UpdateCustomer(CatalogCustomer customer)
    {
        await UpdateAsync(customer);
        return await SaveChangesAsync();
    }

    public async Task<int> DeleteCustomer(CatalogCustomer customer)
    {
        await DeleteAsync(customer);
        return await SaveChangesAsync();
    }
}