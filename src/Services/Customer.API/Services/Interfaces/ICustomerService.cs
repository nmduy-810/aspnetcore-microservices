using Shared.DTOs.Customer;

namespace Customer.API.Services.Interfaces;

public interface ICustomerService
{
    Task<IResult?> GetCustomerByUserNameAsync(string userName);
    Task<IResult> GetCustomerAsync();
    Task<IResult> GetCustomer(int id);
    Task<IResult> CreateCustomer(CreateCustomerDto customerDto);
    Task<IResult> UpdateCustomer(int id, UpdateCustomerDto customerDto);
    Task<IResult> DeleteCustomer(int id);
}