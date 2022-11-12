using System.Text;
using System.Text.Json;
using Benkyoukai.Api.Data;
using RabbitMQ.Client;

namespace Benkyoukai.Api.Services.Common;

public class MessageProducer : IMessageProducer
{
    private readonly IMQConnectionFactory _connectionFactory;

    public MessageProducer(IMQConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void SendMessage<T>(T message, string exchange, string queue)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queue,
            durable: true,
            exclusive: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(
            exchange: exchange,
            routingKey: queue,
            body: body);
    }
}
