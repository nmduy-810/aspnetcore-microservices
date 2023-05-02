using Shared.Enums.Inventory;

namespace Shared.DTOs.Inventory;

public class PurchaseProductDto
{
    public EDocumentType DocumentType => EDocumentType.Purchase;
    
    public string? ItemNo { get; set; } = default!;
    
    public string? DocumentNo { get; set; } = default!;
    
    public string? ExternalDocumentNo { get; set; } = default!;
    
    public int Quantity { get; set; } = default!;
    
    public void SetItemNo(string itemNo) => ItemNo = itemNo;
}