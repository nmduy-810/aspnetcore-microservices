using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Contracts.Messages;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Queries.GetOrders;
using Ordering.Domain.Entities;
using Shared.Services.Email;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISmtpEmailService _smtpEmailService;
    private readonly IMessageProducer _messageProducer;

    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderController(IMediator mediator, ISmtpEmailService smtpEmailService, IMessageProducer messageProducer, IOrderRepository orderRepository, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _smtpEmailService = smtpEmailService;
        _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    private static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
    }

    [HttpGet("{username}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string username)
    {
        var query = new GetOrdersQuery(username);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        var message = new MailRequest()
        {
            Body = "hello",
            Subject = "test",
            ToAddress = "duynguyen8101996@gmail.com"
        };

        await _smtpEmailService.SendEmailAsync(message);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        var addOrder = await _orderRepository.CreateOrder(order);
        var result = _mapper.Map<OrderDto>(addOrder);
        _messageProducer.SendMessage(result);
        return Ok(result);
    }

}