using Shared.Enums.Inventory;

namespace Shared.DTOs.Inventory;

public class InventoryEntryDto
{
    public string Id { get; set; } = default!;
    
    public EDocumentType DocumentType { get; set; }

    public string DocumentNo { get; set; } = default!;

    public string ItemNo { get; set; } = default!;

    public int Quantity { get; set; } = default!;

    public string ExternalDocumentNo { get; set; } = default!;
}