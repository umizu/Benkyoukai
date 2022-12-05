using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Benkyoukai.Email.Services.Email;

public class DeadEmailService
{
    public EventHandler<BasicDeliverEventArgs> ProcessDeadEmail(IModel channel)
    {
        return (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Dead letter received {message}");
            channel.BasicAck(eventArgs.DeliveryTag, false);
        };
    }
}
