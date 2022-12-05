using Benkyoukai.Email.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Benkyoukai.Email.Services.Common;

public class MessageConsumer : IMessageConsumer
{
    private readonly IMQConnectionFactory _connectionFactory;

    public MessageConsumer(IMQConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public EventingBasicConsumer OpenChannel(string queue, string deadLetterRoutingKey = "")
    {
        var connection = _connectionFactory.CreateConnection();
        var channel = connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: "dlx.exchange",
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null);

        var args = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(deadLetterRoutingKey))
        {
            args.Add("x-dead-letter-exchange", "dlx.exchange");
            args.Add("x-dead-letter-routing-key", deadLetterRoutingKey);
        }

        channel.QueueDeclare(
            queue: queue,
            durable: true,
            exclusive: false,
            arguments: args
        );

        var consumer = new EventingBasicConsumer(channel);

        channel.BasicConsume(
            queue: queue,
            autoAck: false,
            consumer: consumer);

        return consumer;
    }
}
