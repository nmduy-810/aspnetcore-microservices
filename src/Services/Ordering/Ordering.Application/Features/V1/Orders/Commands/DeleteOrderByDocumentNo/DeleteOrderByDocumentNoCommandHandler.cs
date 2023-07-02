using MediatR;
using Ordering.Application.Common.Interfaces;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrderByDocumentNo;

public class DeleteOrderByDocumentNoCommandHandler : IRequestHandler<DeleteOrderByDocumentNoCommand, ApiResult<bool>>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderByDocumentNoCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }
    
    public async Task<ApiResult<bool>> Handle(DeleteOrderByDocumentNoCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.GetOrderByDocumentNo(request.DocumentNo);
        if (orderEntity == null) return new ApiSuccessResult<bool>(true);
        
        _orderRepository.DeleteOrder(orderEntity);
        orderEntity.DeletedOrder();
        await _orderRepository.SaveChangesAsync();

        Log.Information("Order {OrderEntityDocumentNo} was successfully deleted", orderEntity.DocumentNo);

        return new ApiSuccessResult<bool>(true);
    }
}