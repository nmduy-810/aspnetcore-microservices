using Basket.API.Services.Interfaces;
using Shared.Configurations;

namespace Basket.API.Services;

public class BasketEmailTemplateService : EmailTemplateService, IEmailTemplateService
{
    public BasketEmailTemplateService(BackgroundJobSettings settings) : base(settings)
    {
    }
    
    public string GenerateReminderCheckoutOrderEmail(string username)
    {
        var checkoutUrl = $"{BackgroundJobSettings.CheckoutUrl}/{BackgroundJobSettings.BasketUrl}/{username}";
        if (checkoutUrl == null) 
            throw new ArgumentNullException(nameof(checkoutUrl));
        
        var emailText = ReadEmailTemplateContent("reminder-checkout-order");
        var emailReplacedText = emailText.Replace("[username]", username)
            .Replace("[checkoutUrl]", checkoutUrl);

        return emailReplacedText;
    }
}