using Benkyoukai.Services.Contracts.Email;
using Benkyoukai.Services.Email.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Benkyoukai.Services.Email.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task<bool> SendEmailAsync(EmailRegisterMessageDto message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_smtpSettings.Username));
        email.To.Add(MailboxAddress.Parse(_smtpSettings.Username));
        email.Subject = message.Subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = message.Body
        };

        using var smtp = new SmtpClient();
        smtp.Connect(_smtpSettings.Host, 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(_smtpSettings.Username, _smtpSettings.Password);
        var response = await smtp.SendAsync(email);
        smtp.Disconnect(true);

        return true;
    }
}
