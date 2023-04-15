using AutoMapper;
using Customer.API.Entities;
using Shared.DTOs.Customer;

namespace Customer.API;

public class MappingProfile : Profile
{
   public MappingProfile()
   {
      CreateMap<CreateCustomerDto, CatalogCustomer>();
      CreateMap<UpdateCustomerDto, CatalogCustomer>();
   }
}