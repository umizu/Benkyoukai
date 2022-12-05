using System.Text;
using System.Text.Json;
using Benkyoukai.Services.Contracts.Email;
using Benkyoukai.Email.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Benkyoukai.Email.Services.Email;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public EventHandler<BasicDeliverEventArgs> ProcessEmail(IModel channel)
    {
        return async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var email = JsonSerializer.Deserialize<EmailRegisterMessageDto>(message)!;
            Console.WriteLine($"Received {message}");
            var sent = await SendEmailAsync(email);

            if (!sent)
            {
                channel.BasicNack(eventArgs.DeliveryTag, false, false);
                return;
            }

            channel.BasicAck(eventArgs.DeliveryTag, false);
        };
    }

    private async Task<bool> SendEmailAsync(EmailRegisterMessageDto message)
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
