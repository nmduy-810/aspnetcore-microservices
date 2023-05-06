using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities;

public class CartItem
{
    [Required]
    [Range(1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}.")]
    public int Quantity { get; set; } = default!;
    
    [Required]
    [Range(0.1, double.PositiveInfinity, ErrorMessage = "The field {0} must be >= {1}.")]
    public decimal ItemPrice { get; set; } = default!;
    public string ItemNo { get; set; } = default!;
    public string ItemName { get; set; } = default!;

    public int AvailableQuantity { get; set; } // So luong hien co

    public void SetAvailableQuantity(int quantity) => (AvailableQuantity) = quantity;
}