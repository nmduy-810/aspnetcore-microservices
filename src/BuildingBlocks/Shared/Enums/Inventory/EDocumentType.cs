namespace Shared.Enums.Inventory;

public enum EDocumentType
{
    All = 0, // filtering
    Purchase = 101, // buy external
    PurchaseInternal = 102, // buy internal
    Sale = 201, // sale external
    SaleInternal = 202, // sale internal
}