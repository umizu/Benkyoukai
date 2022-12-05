using RabbitMQ.Client.Events;

namespace Benkyoukai.Email.Services.Common;

public interface IMessageConsumer
{
    EventingBasicConsumer OpenChannel(string queue, string deadLetterRoutingKey = "");
}
