namespace Shared.DTOs.Inventory;

public class PurchaseProductDto
{
    public string ItemNo { get; set; } = default!;
    
    public string DocumentNo { get; set; } = default!;
    
    public string ExternalDocumentNo { get; set; } = default!;

    public string Quantity { get; set; } = default!;
    
    
}