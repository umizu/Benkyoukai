using RabbitMQ.Client;

namespace Benkyoukai.Api.Data;

public interface IMQConnectionFactory
{
    IConnection CreateConnection();
}
