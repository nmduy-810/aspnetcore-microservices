using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    private const string MethodName = "DeleteOrderCommandHandler";

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("BEGIN {MethodName} - Id: {RequestUserName}", MethodName, request.Id);
        
        var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
        if (orderEntity == null) 
            throw new NotFoundException(nameof(Order), request.Id);
        
        _orderRepository.DeleteOrder(orderEntity);
        await _orderRepository.SaveChangesAsync();

        _logger.Information("Order {OrderEntityId} was successfully deleted", orderEntity.Id);
        
        _logger.Information("END {MethodName} - Id: {RequestUserName}", MethodName, request.Id);


        return Unit.Value;
    }
}