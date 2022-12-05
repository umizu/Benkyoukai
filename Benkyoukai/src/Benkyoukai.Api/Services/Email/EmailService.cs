using Benkyoukai.Api.Dtos.Emails;
using Benkyoukai.Api.Services.Common;

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
