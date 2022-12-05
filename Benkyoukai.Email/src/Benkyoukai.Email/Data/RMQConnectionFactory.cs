using RabbitMQ.Client;

namespace Benkyoukai.Email.Data;

public class RMQConnectionFactory : IMQConnectionFactory
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;

    public RMQConnectionFactory(string hostName, string userName, string password)
    {
        _hostName = hostName;
        _userName = userName;
        _password = password;
    }

    public IConnection CreateConnection()
    {
        return new ConnectionFactory()
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        }.CreateConnection();
    }
}
