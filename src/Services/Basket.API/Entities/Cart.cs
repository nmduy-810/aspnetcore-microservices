namespace Basket.API.Entities;

public class Cart
{
    public Cart() {}

    public Cart(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; } = default!;

    public List<CartItem> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(item => item.ItemPrice * item.Quantity);
}