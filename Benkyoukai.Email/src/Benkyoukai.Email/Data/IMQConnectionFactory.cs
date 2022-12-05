using RabbitMQ.Client;

namespace Benkyoukai.Email.Data;

public interface IMQConnectionFactory
{
    IConnection CreateConnection();
}
