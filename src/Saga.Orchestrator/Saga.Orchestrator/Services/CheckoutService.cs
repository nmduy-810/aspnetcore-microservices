using AutoMapper;
using Saga.Orchestrator.HttpRepositories.Interfaces;
using Saga.Orchestrator.Services.Interfaces;
using Serilog;
using Shared.DTOs.Basket;
using Shared.DTOs.Inventory;
using Shared.DTOs.Order;

namespace Saga.Orchestrator.Services;

public class CheckoutSagaService : ICheckoutSagaService
{
    private readonly IOrderHttpRepository _orderHttpRepository;
    private readonly IBasketHttpRepository _basketHttpRepository;
    private readonly IInventoryHttpRepository _inventoryHttpRepository;
    private readonly IMapper _mapper;
    public CheckoutSagaService(IOrderHttpRepository orderHttpRepository, IBasketHttpRepository basketHttpRepository, IInventoryHttpRepository inventoryHttpRepository, IMapper mapper)
    {
        _orderHttpRepository = orderHttpRepository;
        _basketHttpRepository = basketHttpRepository;
        _inventoryHttpRepository = inventoryHttpRepository;
        _mapper = mapper;
    }
    
    public async Task<bool> CheckoutOrder(string username, BasketCheckoutDto basketCheckout)
    {
        // Get cart from BasketHttpRepository
        Log.Information("Start: Get Cart {Username}", username);

        var cart = await _basketHttpRepository.GetBasket(username);
        if (cart == null) return false;
        Log.Information("End: Get Cart {Username} success", username);

        // Create Order from OrderHttpRepository
        Log.Information($"Start: Create Order");

        var order = _mapper.Map<CreateOrderDto>(basketCheckout);
        order.TotalPrice = cart.TotalPrice;
        // Get Order by order id
        var orderId = await _orderHttpRepository.CreateOrder(order);
        if (orderId < 0) return false;
        var addedOrder = await _orderHttpRepository.GetOrder(orderId);
        Log.Information($"End: Created Order success, Order Id: {orderId} - Document No - {addedOrder.DocumentNo}");

        var inventoryDocumentNos = new List<string>();
        bool result;
        try
        {
            // Sales Items from InventoryHttpRepository
            foreach (var item in cart.Items)
            {
                Log.Information("Start: Sale Item No: {ItemItemNo} - Quantity: {ItemQuantity}", item.ItemNo, item.Quantity);

                var saleOrder = new SalesProductDto(addedOrder.DocumentNo, item.Quantity);
                saleOrder.SetItemNo(item.ItemNo);
                var documentNo = await _inventoryHttpRepository.CreateSalesOrder(saleOrder);
                inventoryDocumentNos.Add(documentNo);
                Log.Information("End: Sale Item No: {ItemNo} - Quantity: {Quantity} - Document No: {DocumentNo}", item.ItemNo, item.Quantity, documentNo);
            }

            // delete basket
            result = await _basketHttpRepository.DeleteBasket(username);
        }
        catch (Exception e)
        {
           Log.Error(e, "{Message}",e.Message);
           
           // Rollback checkout order
           await RollbackCheckoutOrder(username, addedOrder.Id, inventoryDocumentNos);
           result = false;
        }

        return result;
    }

    private async Task RollbackCheckoutOrder(string username, long orderId, List<string> inventoryDocumentNos)
    {
        var deletedDocumentNos = new List<string>();
        
        Log.Information("Start: Delete Order Id: {OrderId}", orderId);
        
        // delete order by order's id, order's document no
        await _orderHttpRepository.DeleteOrder(orderId);
        
        Log.Information("End: Delete Order Id: {OrderId}", orderId);
        
        foreach (var documentNo in inventoryDocumentNos)
        {
            await _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo);
            deletedDocumentNos.Add(documentNo);
        }
        Log.Information("End: Deleted Inventory Document Nos: {Join}", string.Join(", ", inventoryDocumentNos));
    }
}