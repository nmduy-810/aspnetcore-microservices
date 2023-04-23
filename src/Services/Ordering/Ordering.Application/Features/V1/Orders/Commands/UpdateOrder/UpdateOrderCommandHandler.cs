using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    private const string MethodName = "UpdateOrderCommandHandler";

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
        
        if (orderEntity is null) 
            throw new NotFoundException(nameof(Order), request.Id);
        
        _logger.Information("BEGIN: {MethodName} - Order: {RequestId}", MethodName, request.Id);
        
        orderEntity = _mapper.Map(request, orderEntity);
        var updatedOrder = await _orderRepository.UpdateOrder(orderEntity);
        await _orderRepository.SaveChangesAsync();
        
        _logger.Information("Order {RequestId} was successfully updated", request.Id);
        
        var result = _mapper.Map<OrderDto>(updatedOrder);
        
        _logger.Information("END: {MethodName} - Order: {RequestId}", MethodName, request.Id);
        
        return new ApiSuccessResult<OrderDto>(result);
    }
}