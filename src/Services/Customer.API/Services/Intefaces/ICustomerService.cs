namespace Customer.API.Services.Intefaces;

public interface ICustomerService
{
    Task<IResult> GetCustomerByUserNameAsync(string username);
    Task<IResult> GetCustomersAsync();
}