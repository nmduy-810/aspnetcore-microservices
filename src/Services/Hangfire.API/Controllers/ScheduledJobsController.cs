using System.ComponentModel.DataAnnotations;
using Hangfire.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.ScheduledJob;

namespace Hangfire.API.Controllers;

[ApiController]
[Route("api/scheduled-jobs")]
public class ScheduledJobsController : ControllerBase
{
    private readonly IBackgroundJobService _backgroundJobService;
    
    public ScheduledJobsController(IBackgroundJobService backgroundJobService)
    {
        _backgroundJobService = backgroundJobService ?? throw new ArgumentNullException(nameof(backgroundJobService));
    }

    [HttpPost]
    [Route("send-email-reminder-checkout-order")]
    public IActionResult SendReminderCheckoutOrderEmail([FromBody] ReminderCheckoutOrderDto model)
    {
        var jobId = _backgroundJobService.SendEmailContent(model.Email, model.Subject, model.EmailContent, model.EnqueueAt);
        return Ok(jobId);
    }

    [HttpDelete]
    [Route("delete/jobId/{id}")]
    public IActionResult DeleteJobId([Required] string id)
    {
        var result = _backgroundJobService.ScheduledJobService.Delete(id);
        return Ok(result);
    }
}