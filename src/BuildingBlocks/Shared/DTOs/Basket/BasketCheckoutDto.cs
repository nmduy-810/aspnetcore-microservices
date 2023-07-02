namespace Shared.DTOs.Basket;

public class BasketCheckoutDto
{
    public string UserName { get; set; } = default!;
    
    public decimal TotalPrice { get; set; } = default!;
    
    public string FirstName { get; set; } = default!;
    
    public string LastName { get; set; } = default!;

    public string EmailAddress { get; set; } = default!;
    
    public string? ShippingAddress { get; set; }
    
    private string? _invoiceAddress;
    public string? InvoiceAddress
    {
        get => _invoiceAddress;
        set => _invoiceAddress = value ?? ShippingAddress;
    }
}