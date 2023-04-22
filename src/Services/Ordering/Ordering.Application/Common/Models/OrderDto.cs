using AutoMapper;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;

namespace Ordering.Application.Common.Models;

public class OrderDto : IMapFrom<Order>
{
    public long Id { get; set; }
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public string InvoiceAddress { get; set; } = default!;
    public OrderStatusEnum Status { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderDto>().ReverseMap();
    }
}