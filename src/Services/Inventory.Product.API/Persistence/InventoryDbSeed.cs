using Inventory.Product.API.Entity;
using Inventory.Product.API.Extensions;
using MongoDB.Driver;
using Shared.Enums.Inventory;

namespace Inventory.Product.API.Persistence;

public class InventoryDbSeed
{
    public async Task SeedDataAsync(IMongoClient mongoClient, DatabaseSettings databaseSettings)
    {
        var databaseName = databaseSettings.DatabaseName;
        var database = mongoClient.GetDatabase(databaseName);
        var inventoryCollection = database.GetCollection<InventoryEntry>("InventoryEntries");
        if (await inventoryCollection.EstimatedDocumentCountAsync() == 0) // if no record in database
        {
            await inventoryCollection.InsertManyAsync(GetPreconfiguredInventoryEntries());
        }
    }

    private static IEnumerable<InventoryEntry> GetPreconfiguredInventoryEntries()
    {
        return new List<InventoryEntry>()
        {
            new()
            {
                Quantity = 10,
                DocumentNo = Guid.NewGuid().ToString(),
                ItemNo = "Apple",
                ExternalDocumentNo = Guid.NewGuid().ToString(),
                DocumentType = EDocumentType.Purchase
            },
            new()
            {
                Quantity = 10,
                DocumentNo = Guid.NewGuid().ToString(),
                ItemNo = "Samsung",
                ExternalDocumentNo = Guid.NewGuid().ToString(),
                DocumentType = EDocumentType.Purchase
            },
        };
    }
}