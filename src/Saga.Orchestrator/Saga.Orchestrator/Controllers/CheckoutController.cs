using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;

namespace Saga.Orchestrator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ICheckoutSagaService _sagaService;
    
    public CheckoutController(ICheckoutSagaService sagaService)
    {
        _sagaService = sagaService;
    }

    [HttpPost]
    [Route("{username}")]
    public async Task<IActionResult> CheckoutOrder([Required] string username, [FromBody] BasketCheckoutDto model)
    {
        model.UserName = username;
        var result = await _sagaService.CheckoutOrder(username, model);
        return Ok(result);
    }
}