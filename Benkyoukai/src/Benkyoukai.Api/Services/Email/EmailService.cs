using Benkyoukai.Api.Services.Common;
using Benkyoukai.Services.Contracts.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Benkyoukai.Api.Services.Email;

public class EmailService : IEmailService
{
    private readonly IMessageProducer _messageProducer;
    private const string Queue = "email";
    private const string DeadLetterRoutingKey = "dlx.email";

    public EmailService(IMessageProducer messageProducer)
    {
        _messageProducer = messageProducer;
    }

    public void SendRegistrationEmail(EmailRegisterMessageDto message)
    {
        _messageProducer.SendMessage(
            message: message,
            exchange: "",
            queue: Queue,
            deadLetterRoutingKey: DeadLetterRoutingKey);
    }
}
