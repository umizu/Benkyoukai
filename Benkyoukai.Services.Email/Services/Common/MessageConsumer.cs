using Benkyoukai.Services.Email.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Benkyoukai.Services.Email.Services.Common;

public class MessageConsumer : IMessageConsumer
{
    private readonly IMQConnectionFactory _connectionFactory;

    public MessageConsumer(IMQConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public EventingBasicConsumer CreateEmailRegistrationChannel()
    {
        var connection = _connectionFactory.CreateConnection();
        var channel = connection.CreateModel();
        
        channel.QueueDeclare(
            queue: "email",
            durable: true,
            exclusive: false
        );

        var consumer = new EventingBasicConsumer(channel);

        channel.BasicConsume(
            queue: "email",
            autoAck: false,
            consumer: consumer);

        return consumer;
    }
}
