using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Features.V1.Orders.Common;
using Ordering.Domain.Entities;
using Shared.DTOs.Order;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder;

public class CreateOrderCommand : CreateOrUpdateCommand, IRequest<ApiResult<long>>, IMapFrom<Order>
{
    public string UserName { get; set; } = default!;

    public new void Mapping(Profile profile)
    {
        profile.CreateMap<CreateOrderDto, CreateOrderCommand>();
        profile.CreateMap<CreateOrderCommand, Order>();
        profile.CreateMap<BasketCheckoutEvent, CreateOrderCommand>();
    }
}