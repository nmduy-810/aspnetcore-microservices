using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces;

public interface ICustomerRepository : IRepositoryQueryBase<CatalogCustomer, int, CustomerContext>
{
    Task<CatalogCustomer?> GetCustomerByUserNameAsync(string username);
    Task<IEnumerable<CatalogCustomer>> GetCustomersAsync();
}