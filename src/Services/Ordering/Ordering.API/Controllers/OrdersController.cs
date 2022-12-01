using System.ComponentModel.DataAnnotations;
using System.Net;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrder;
using Ordering.Application.Features.V1.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.V1.Orders.Queries.GetOrders;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISmtpEmailService _smtpEmailService;
    
    public OrdersController(IMediator mediator, ISmtpEmailService smtpEmailService)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _smtpEmailService = smtpEmailService;
    }

    // Define controller have route names
    private static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
        public const string CreateOrders = nameof(CreateOrders);
        public const string UpdateOrders = nameof(UpdateOrders);
        public const string DeleteOrders = nameof(DeleteOrders);
    }

    [HttpGet("{username}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByUserName([Required] string username)
    {
        var query = new GetOrdersQuery(username);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost(Name = RouteNames.CreateOrders)]
    [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:long}", Name = RouteNames.UpdateOrders)]
    [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderDto>> UpdateOrder([Required] long id, [FromBody] UpdateOrderCommand command)
    {
        command.SetId(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{id:long}", Name = RouteNames.DeleteOrders)]
    [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> DeleteOrder([Required] long id)
    {
        var command = new DeleteOrderCommand(id);
        var result = await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        var message = new MailRequest()
        {
            Body = "<h1>Hello<h1>", Subject = "Test", ToAddress = "duynguyendev810@gmail.com"
        };
        await _smtpEmailService.SendEmailAsync(message);
        return Ok();
    }
}