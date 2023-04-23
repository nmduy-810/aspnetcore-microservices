using AutoMapper;
using Contracts.Services;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ISmtpEmailService _emailService;
    private readonly ILogger _logger;

    private const string MethodName = "CreateOrderCommandHandler";

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ISmtpEmailService emailService, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information("BEGIN {MethodName} - Username: {RequestUserName}", MethodName, request.UserName);

        var orderEntity = _mapper.Map<Order>(request);
        var addOrder = await _orderRepository.CreateOrder(orderEntity);
        await _orderRepository.SaveChangesAsync();
        
        _logger.Information("Order {Id} is successfully created", addOrder.Id);

        //SendEmailAsync(addOrder, cancellationToken);
        
        _logger.Information("END {MethodName} - Username: {RequestUserName}", MethodName, request.UserName);
        return new ApiSuccessResult<long>(addOrder.Id);
    }
}