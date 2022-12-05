namespace Benkyoukai.Api.Services.Common;

public interface IMessageProducer
{
    void SendMessage<T>(T message, string exchange, string queue, string deadLetterRoutingKey);
}
