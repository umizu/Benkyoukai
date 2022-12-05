using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Benkyoukai.Email.Services.Email;

public interface IEmailService
{
    EventHandler<BasicDeliverEventArgs> ProcessEmail(IModel channel);
}
