using Inventory.Grpc.Client;
using Serilog;

namespace Basket.API.GrpcServices;

public class StockItemGrpcService
{
    private readonly StockProtoService.StockProtoServiceClient _stockProtoServiceClient;
    public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoServiceClient)
    {
        _stockProtoServiceClient = stockProtoServiceClient ?? throw new ArgumentNullException(nameof(stockProtoServiceClient));
    }

    public async Task<StockModel> GetStock(string itemNo)
    {
        try
        {
            Log.Information("BEGIN: Get Stock StockItemGrpcService Item No: {ItemNo}", itemNo);
            var stockItemRequest = new GetStockRequest { ItemNo = itemNo };
            var result =  await _stockProtoServiceClient.GetStockAsync(stockItemRequest);
            Log.Information("END: Get Stock StockItemGrpcService Item No: {ItemNo}", itemNo);
            return result;
        }
        catch (Exception e)
        {
            Log.Information("StockItemGrpcService failed: {Message}", e.Message);
            throw;
        }
    }
}