using Contracts.Services;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using MimeKit;
using Shared.Services.Email;
using Serilog;

namespace Infrastructure.Services;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly ILogger _logger;
    private readonly SmtpEmailSettings _settingses;
    private readonly SmtpClient _smtpClient;

    public SmtpEmailService(ILogger logger, SmtpEmailSettings settingses)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settingses = settingses ?? throw new ArgumentNullException(nameof(settingses));
        _smtpClient = new SmtpClient();
    }

    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var emailMessage = new MimeMessage
        {
            Sender = new MailboxAddress(_settingses.DisplayName, request.From),
            Subject = request.Subject,
            Body = new BodyBuilder
            {
                HtmlBody = request.Body
            }.ToMessageBody()
        };
        
        if (request.ToAddresses.Any())
        {
            foreach (var toAddress in request.ToAddresses)
            {
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
        }
        else
        {
            var toAddress = request.ToAddress;
            emailMessage.To.Add(MailboxAddress.Parse(toAddress));
        }
        
        try
        {
            await _smtpClient.ConnectAsync(_settingses.SmtpServer, _settingses.Port, 
                _settingses.UseSsl, cancellationToken);
            await _smtpClient.AuthenticateAsync(_settingses.Username, _settingses.Password, cancellationToken);
            await _smtpClient.SendAsync(emailMessage, cancellationToken);
            await _smtpClient.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unhandled send email: {EMessage}", e.Message);
        }
        finally
        {
            await _smtpClient.DisconnectAsync(true, cancellationToken);
            _smtpClient.Dispose();
        }
    }
    
    private MimeMessage GetMimeMessage(MailRequest request)
    {
        var emailMessage = new MimeMessage
        {
            Sender = new MailboxAddress(_settingses.DisplayName, _settingses.From),
            Subject = request.Subject,
            Body = new BodyBuilder
            {
                HtmlBody = request.Body
            }.ToMessageBody()
        };
        
        if (request.ToAddresses.Any())
        {
            foreach (var toAddress in request.ToAddresses)
            {
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
        }
        else
        {
            var toAddress = request.ToAddress;
            emailMessage.To.Add(MailboxAddress.Parse(toAddress));
        }

        return emailMessage;
    }
    
    public void SendEmail(MailRequest request)
    {
        var emailMessage = GetMimeMessage(request);
        try
        {
            _smtpClient.Connect(_settingses.SmtpServer, _settingses.Port, 
                _settingses.UseSsl);
            _smtpClient.Authenticate(_settingses.Username, _settingses.Password);
            _smtpClient.Send(emailMessage);
            _smtpClient.Disconnect(true);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{ExMessage}", ex.Message);
        }
        finally
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }
    }
}


