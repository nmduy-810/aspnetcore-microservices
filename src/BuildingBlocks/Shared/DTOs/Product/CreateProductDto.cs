namespace Shared.DTOs.Product;

public class CreateProductDto
{
    public string No { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Summary { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }
}