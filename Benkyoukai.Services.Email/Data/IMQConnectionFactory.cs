using RabbitMQ.Client;

namespace Benkyoukai.Services.Email.Data;

public interface IMQConnectionFactory
{
    IConnection CreateConnection();
}
