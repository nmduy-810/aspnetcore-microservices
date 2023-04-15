using AutoMapper;
using Customer.API.Entities;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Shared.DTOs.Customer;

namespace Customer.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    
    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IResult?> GetCustomerByUserNameAsync(string userName) =>
        Results.Ok(await _customerRepository.GetCustomerByUserNameAsync(userName));

    public async Task<IResult> GetCustomerAsync() => 
        Results.Ok(await _customerRepository.GetCustomerAsync());

    public async Task<IResult> GetCustomer(int id) => Results.Ok(await _customerRepository.GetCustomerById(id));

    public async Task<IResult> CreateCustomer(CreateCustomerDto customerDto)
    {
        var customer = _mapper.Map<CatalogCustomer>(customerDto);
        return Results.Ok(await _customerRepository.CreateCustomer(customer));
    }

    public async Task<IResult> UpdateCustomer(int id, UpdateCustomerDto customerDto)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        if (customer == null) 
            return default!;
        
        var currentCustomer = _mapper.Map(customerDto, customer);
        return Results.Ok(await _customerRepository.UpdateCustomer(currentCustomer));
    }

    public async Task<IResult> DeleteCustomer(int id)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        return customer != null ? Results.Ok(await _customerRepository.DeleteCustomer(customer)) : default!;
    }
}