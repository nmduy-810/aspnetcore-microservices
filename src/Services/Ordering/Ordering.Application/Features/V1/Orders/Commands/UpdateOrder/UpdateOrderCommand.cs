using MediatR;
using Ordering.Application.Common.Models;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest<ApiResult<OrderDto>>
{
    public long Id { get; private set; }

    public void SetId(long id)
    {
        Id = id;
    }
    
    public string UserName { get; set; } = default!;

    public decimal TotalPrice { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;

    public string ShippingAddress { get; set; } = default!;

    public string InvoiceAddress { get; set; } = default!;
}