namespace Basket.API.Entities;

public class Cart
{
    public string UserName { get; set; } = default!;
    public List<CartItem> CartItems { get; set; } = new();
    
    public Cart()
    {
        
    }

    public Cart(string userName)
    {
        UserName = userName;
    }
    
    public decimal TotalPrice => CartItems.Sum(item => item.ItemPrice * item.Quantity);
}