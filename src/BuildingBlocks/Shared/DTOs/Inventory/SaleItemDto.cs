using Shared.Enums.Inventory;

namespace Shared.DTOs.Inventory;

public class SaleItemDto
{
    public string ItemNo { get; set; } = default!;
    
    public int Quantity { get; set; }
    
    public EDocumentType DocumentType => EDocumentType.Sale;
}