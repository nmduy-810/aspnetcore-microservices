namespace Shared.Configurations;

public class BackgroundJobSettings
{
    public string HangfireUrl { get; set; } = default!;
    
    public string CheckoutUrl { get; set; }  = default!;
    
    public string BasketUrl { get; set; }  = default!;
    
    public string ScheduledJobUrl { get; set; }  = default!;
}