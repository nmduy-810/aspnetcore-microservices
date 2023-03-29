using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Product;

public class CreateOrUpdateProductDto
{
    [Required]
    [MaxLength(250, ErrorMessage = "Maximum length for Product Name is 250 characters")]
    public string Name { get; set; } = default!;

    [MaxLength(255, ErrorMessage = "Maximum length for Product Summary is 255 characters")]
    public string Summary { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }
}