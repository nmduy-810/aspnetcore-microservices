using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;

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
    public string Status { get; set; } = default!;
}