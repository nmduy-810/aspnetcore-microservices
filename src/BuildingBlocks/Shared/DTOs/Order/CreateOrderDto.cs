namespace Shared.DTOs.Order;

public class CreateOrderDto
{
    public string UserName { get; set; } = default!;
    
    public decimal TotalPrice { get; set; } = default!;
    
    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;
    
    public string EmailAddress { get; set; } = default!;
    
    public string ShippingAddress { get; set; } = default!;

    public string InvoiceAddress { get; set; } = default!;
}