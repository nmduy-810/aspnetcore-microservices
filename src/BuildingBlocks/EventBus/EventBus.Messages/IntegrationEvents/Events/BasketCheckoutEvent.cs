using EventBus.Messages.IntegrationEvents.Interfaces;

namespace EventBus.Messages.IntegrationEvents.Events;

public record BasketCheckoutEvent() : IntegrationBaseEvent, IBasketCheckoutEvent
{
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public string InvoiceAddress { get; set; } = default!;
}