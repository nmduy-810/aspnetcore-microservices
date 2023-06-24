namespace Basket.API.Entities;

public class Cart
{
    public string UserName { get; set; } = default!;
    
    public string EmailAddress { get; set; } = default!;
    public List<CartItem> CartItems { get; set; } = new();

    public Cart(string userName)
    {
        UserName = userName;
    }
    
    public Cart()
    {
        
    }
    
    public decimal TotalPrice => CartItems.Sum(item => item.ItemPrice * item.Quantity);

    public DateTimeOffset LastModifiedDate { get; set; } = DateTimeOffset.UtcNow;

    public string? JobId { get; set; }
}