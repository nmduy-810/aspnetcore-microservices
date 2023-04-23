using AutoMapper;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.V1.Orders.Common;

public class CreateOrUpdateCommand : IMapFrom<Order>
{
    public decimal TotalPrice { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public string InvoiceAddress { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrUpdateCommand, Order>();
    }
}