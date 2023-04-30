using Product.API.Entities;
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence;

public abstract class ProductContextSeed
{
    public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
    {
        if (!productContext.Products.Any())
        {
            productContext.AddRange(GetCatalogProducts());
            await productContext.SaveChangesAsync();
            logger.Information("Seeded data for Product DB associated with context {DbContextName}",
                nameof(ProductContext));
        }
    }
    
    private static IEnumerable<CatalogProduct> GetCatalogProducts()
    {
        return new List<CatalogProduct>
        {
            new()
            {
                No = "Apple",
                Name = "Iphone 13 Pro Max",
                Summary = "Expensive smart phone",
                Description = "This is a smartphone very beautiful",
                Price = (decimal)177940.49
            },
            new()
            {
                No = "Samsung",
                Name = "Samsung",
                Summary = "Cheap smart phone",
                Description = "A smartphone with very cheap, bad screen",
                Price = (decimal)114728.21
            }
        };
    }
}