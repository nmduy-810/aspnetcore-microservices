using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.Services.Email;

public class MailRequest
{
    [EmailAddress]
    public string From { get; set; } = default!;
    
    [EmailAddress]
    public string ToAddress { get; set; } = default!;
    
    public IEnumerable<string> ToAddresses { get; set; } = new List<string>();
    public string Subject { get; set; } = default!;
    
    [Required]
    public string Body { get; set; } = default!;
    
    public IFormFileCollection Attachments { get; set; } = default!;
}