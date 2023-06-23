using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;
    private readonly StockItemGrpcService _stockItemGrpcService;
    
    public BasketController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IMapper mapper, StockItemGrpcService stockItemGrpcService)
    {
        _basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _stockItemGrpcService = stockItemGrpcService ?? throw new ArgumentNullException(nameof(stockItemGrpcService));
    }

    [HttpGet("{username}", Name = "GetBasket")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> GetBasketByUserName([Required] string username)
    {
        var result = await  _basketRepository.GetBasketByUserName(username);
        return Ok(result ?? new Cart());
    }

    [HttpPost(Name = "UpdateBasket")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart cart)
    {
        // Communication with Inventory.Grpc and check quantity available of products
        foreach (var item in cart.CartItems)
        {
            var stock = await _stockItemGrpcService.GetStock(item.ItemNo);
            item.SetAvailableQuantity(stock.Quantity);
        }
        
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddHours(1)) // Set key tồn tại trong 1 giờ
            .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Kiểm tra xem trong 5 phút có hoạt động nào không

        var result = await _basketRepository.UpdateBasket(cart, options);
        return Ok(result);
    }

    [HttpDelete("{username}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteBasket([Required] string username)
    {
        var result = await _basketRepository.DeleteBasketFromUserName(username);
        return Ok(result);
    }

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        /* Lấy giỏ hàng (basket) từ cơ sở dữ liệu thông qua _basketRepository.GetBasketByUserName()
        bằng cách sử dụng tên người dùng trong basketCheckout.UserName. */
        var basket = await _basketRepository.GetBasketByUserName(basketCheckout.UserName);
        
        /* Nếu không tìm thấy giỏ hàng phù hợp, phương thức sẽ trả về kết quả NotFound(). */
        if (basket == null) return NotFound();
        
        // Publish checkout event to event bus message
        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        
        /* Đặt giá trị TotalPrice của eventMessage bằng giá trị TotalPrice của giỏ hàng.
        Điều này đảm bảo rằng tổng giá của đơn hàng sẽ không bị thay đổi do hành vi không mong muốn hoặc lỗi từ phía người dùng. */
        eventMessage.TotalPrice = basket.TotalPrice;
        
        // Publish message (điều này có nghĩa cac dịch vụ khác trong hệ thống sẽ lắng nghe sự kiện này và xử lý các nhiệm vụ liên quan)
        await _publishEndpoint.Publish(eventMessage);
        
        // remove the basket
        await _basketRepository.DeleteBasketFromUserName(basketCheckout.UserName);
        return Accepted();
    }
}