using Contracts.ScheduleJobs;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Hangfire.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WelcomeController : ControllerBase
{
    private readonly IScheduledJobService _jobService;

    public WelcomeController(IScheduledJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Welcome()
    {
        var jobId = _jobService.Enqueue(() => ResponseWelcome("Welcome to Hangfire API"));
        return Ok($"Job ID: {jobId} - Enqueue Job");
    }
   
    [HttpPost]
    [Route("[action]")]
    public IActionResult DelayedWelcome()
    {
        var seconds = 5;
        var jobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"), 
            TimeSpan.FromSeconds(seconds));
        return Ok($"Job ID: {jobId} - Delayed Job");
    }
   
    [HttpPost]
    [Route("[action]")]
    public IActionResult WelcomeAt()
    {
        var enqueueAt = DateTimeOffset.UtcNow.AddSeconds(10);
        var jobId = _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"), 
            enqueueAt);
        return Ok($"Job ID: {jobId} - Schedule Job");
    }
   
    [HttpPost]
    [Route("[action]")]
    public IActionResult ConfirmedWelcome()
    {
        const int timeInSeconds = 5;
        var parentJobId =
            _jobService.Schedule(() => ResponseWelcome("Welcome to Hangfire API"), TimeSpan.FromSeconds(5));

        var jobId = _jobService.ContinueQueueWith(parentJobId, 
            () => ResponseWelcome("Welcome message is sent"));
      
        return Ok($"Job ID: {jobId} - Confirmed Welcome will be sent in {timeInSeconds} seconds");
    }

    [NonAction]
    public void ResponseWelcome(string text)
    {
        Log.Information(text);
    }
}