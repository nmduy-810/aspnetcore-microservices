using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Shared.ScheduledJob;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCacheService;
    private readonly ISerializeService _serializeService;
    private readonly ILogger _logger;
    private readonly BackgroundJobHttpService _backgroundJobHttpService;
    private readonly IEmailTemplateService _emailTemplateService;
    
    public BasketRepository(IDistributedCache redisCacheService, ISerializeService serializeService, ILogger logger, BackgroundJobHttpService backgroundJobHttpService, IEmailTemplateService emailTemplateService)
    {
        _redisCacheService = redisCacheService;
        _serializeService = serializeService;
        _logger = logger;
        _backgroundJobHttpService = backgroundJobHttpService;
        _emailTemplateService = emailTemplateService;
    }

    public async Task<Cart?> GetBasketByUserName(string userName)
    {
        _logger.Information("BEGIN: GetBasketByUserName {UserName}", userName);
        var basket = await _redisCacheService.GetStringAsync(userName);
        _logger.Information("END: GetBasketByUserName {UserName}", userName);
        
        return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
    }

    public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions? options = null)
    {
        _logger.Information("BEGIN: UpdateBasket {UserName}", cart.UserName);
        
        if (options != null)
            await _redisCacheService.SetStringAsync(cart.UserName, _serializeService.Serialize(cart), options);
        else
            await _redisCacheService.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
        
        _logger.Information("END: UpdateBasket {UserName}", cart.UserName);

        try
        {
            await TriggerSendEmailReminderCheckout(cart);
        }
        catch (Exception e)
        {
            _logger.Error(e, "{Message}", e.Message);
        }

        return await GetBasketByUserName(cart.UserName);
    }

    private async Task TriggerSendEmailReminderCheckout(Cart cart)
    {
        var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail(cart.UserName);

        var model = new ReminderCheckoutOrderDto(cart.EmailAddress, "Reminder checkout", emailTemplate,
            DateTimeOffset.UtcNow.AddSeconds(60));

        var uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/send-email-reminder-checkout-order";
        
        var response = await _backgroundJobHttpService.Client.PostAsJson(uri, model);
        if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
        {
            var jobId = await response.ReadContentAs<string>();
            if (!string.IsNullOrEmpty(jobId))
            {
                cart.JobId = jobId;
                await _redisCacheService.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
            }
        }
    }

    public async Task<bool> DeleteBasketFromUserName(string userName)
    {
        _logger.Information("BEGIN: DeleteBasketFromUserName {UserName}", userName);
        try
        {
            await _redisCacheService.RemoveAsync(userName);
            _logger.Information("END: DeleteBasketFromUserName {UserName}", userName);
            return true;
        }
        catch (Exception e)
        {
            _logger.Error(e, "DeleteBasketFromUserName: {Message}", e.Message);
            return false;
        }
    }
}