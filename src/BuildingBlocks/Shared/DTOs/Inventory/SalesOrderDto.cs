namespace Shared.DTOs.Inventory;

public class SalesOrderDto
{
    // Order's Document No
    public string OrderNo { get; set; } = default!;

    public List<SaleItemDto> SaleItems { get; set; } = default!;
}