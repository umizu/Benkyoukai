using RabbitMQ.Client.Events;

namespace Benkyoukai.Services.Email.Services.Common;

public interface IMessageConsumer
{
    EventingBasicConsumer CreateEmailRegistrationChannel();
}
