using Shared.Enums.Order;

namespace Shared.DTOs.Order;

public class OrderDto
{
    public long Id { get; set; }
    
    public string UserName { get; set; } = default!;

    public string DocumentNo { get; set; } = new Guid().ToString();

    public decimal TotalPrice { get; set; }
    
    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;
    
    public string EmailAddress { get; set; } = default!;
    
    public string ShippingAddress { get; set; } = default!;
    
    public string InvoiceAddress { get; set; } = default!;

    public OrderStatusEnum Status { get; set; }
}