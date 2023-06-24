namespace Shared.DTOs.Basket;

public class CartItemDto
{
    public int Quantity { get; set; } = default!;
    
    public decimal ItemPrice { get; set; } = default!;
    
    public string ItemNo { get; set; } = default!;
    
    public string ItemName { get; set; } = default!;

    public int AvailableQuantity { get; set; } // So luong hien co

    public void SetAvailableQuantity(int quantity) => (AvailableQuantity) = quantity;
}