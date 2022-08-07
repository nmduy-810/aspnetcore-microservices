using Microsoft.AspNetCore.Mvc;
using Product.API.Repositories.Interfaces;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _repository.GetProducts();
        return Ok(result);
    }
}