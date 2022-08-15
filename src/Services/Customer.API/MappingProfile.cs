using AutoMapper;
using Customer.API.Entities;
using Infrastructure.Mappings;
using Shared.DTOs.Customer;

namespace Customer.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogCustomer, CustomerDto>();
        CreateMap<UpdateCustomerDto, CatalogCustomer>().IgnoreAllNonExisting();
    }
}