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

        if (!string.IsNullOrEmpty(basket))
        {
            var result = _serializeService.Deserialize<Cart>(basket);
            if (result == null) 
                return result;
            
            var totalPrice = result.TotalPrice;
            _logger.Information("END: GetBasketByUserName {Username} - Total Price: {TotalPrice}", userName, totalPrice);

            return result;
        }
        
        return null;
    }

    public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions? options = null)
    {
        // Xoa nhung job cu neu user bam checkout nhieu lan, nhu vay hang fire chi gui 1 email latest
        await DeleteReminderCheckoutOrder(cart.UserName);
        
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
            DateTimeOffset.UtcNow.AddSeconds(30));

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

    private async Task DeleteReminderCheckoutOrder(string userName)
    {
        var cart = await GetBasketByUserName(userName);
        if (cart == null || string.IsNullOrEmpty(cart.JobId))
            return;

        var jobId = cart.JobId;
        var uri = $"{_backgroundJobHttpService.ScheduledJobUrl}/delete/jobId/{jobId}";
        await _backgroundJobHttpService.Client.DeleteAsync(uri);
        _logger.Information("DeleteReminderCheckoutOrder: Deleted JobId {JobId}", jobId);
    }

    public async Task<bool> DeleteBasketFromUserName(string userName)
    {
        // Neu user da huy het basket, xoa job di de he thong khong phai gui mail nua
        await DeleteReminderCheckoutOrder(userName);
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