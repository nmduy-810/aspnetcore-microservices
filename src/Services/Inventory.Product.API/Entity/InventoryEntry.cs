using Inventory.Product.API.Entity.Abstraction;
using Inventory.Product.API.Extensions;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enums.Inventory;

namespace Inventory.Product.API.Entity;

[BsonCollection("InventoryEntries")]
public sealed class InventoryEntry : MongoEntity
{
    public InventoryEntry()
    {
        DocumentType = EDocumentType.Purchase;
        DocumentNo = Guid.NewGuid().ToString();
        ExternalDocumentNo = Guid.NewGuid().ToString();
    }
    
    public InventoryEntry(string id) => (Id) = id;

    [BsonElement("documentType")]
    public EDocumentType DocumentType { get; set; }

    [BsonElement("documentNo")]
    public string DocumentNo { get; set; } = default!;

    [BsonElement("itemNo")]
    public string ItemNo { get; set; } = default!;

    [BsonElement("quantity")]
    public int Quantity { get; set; } = default!;

    [BsonElement("externalDocumentNo")]
    public string ExternalDocumentNo { get; set; } = default!;
}