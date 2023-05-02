namespace Infrastructure.Extensions;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public readonly string? CollectionName;

    public BsonCollectionAttribute(string? collectionName)
    {
        CollectionName = collectionName;
    }
}