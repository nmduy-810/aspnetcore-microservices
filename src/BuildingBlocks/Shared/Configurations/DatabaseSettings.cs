namespace Shared.Configurations;

public class DatabaseSettings
{
    public string DbProvider { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
}