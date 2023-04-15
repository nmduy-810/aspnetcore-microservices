using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces;

public interface ICustomerRepository : IRepositoryBaseAsync<CatalogCustomer, int, CustomerContext>
{
    Task<CatalogCustomer> GetCustomerByUserNameAsync(string userName);
    Task<IEnumerable<CatalogCustomer>> GetCustomerAsync();
    Task<CatalogCustomer?> GetCustomerById(int id);
    Task<int> CreateCustomer(CatalogCustomer customer);
    Task<int> UpdateCustomer(CatalogCustomer customer);
    Task<int> DeleteCustomer(CatalogCustomer customer);
}