using AutoMapper;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<ApiResult<long>>, IMapFrom<Order>
{
    public string UserName { get; set; } = default!;

    public decimal TotalPrice { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public string ShippingAddress { get; set; } = default!;

    public string InvoiceAddress { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrderCommand, Order>();
    }
}