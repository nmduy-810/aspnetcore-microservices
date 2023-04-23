using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities;

public class BasketCheckout
{
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;

    [Required] 
    public string ShippingAddress { get; set; } = default!;
    private string _invoiceAddress = default!;
    public string? InvoiceAddress
    {
        get => _invoiceAddress;
        set => _invoiceAddress = value ?? ShippingAddress;
    }
}