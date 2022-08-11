using Benkyoukai.Data;
using Benkyoukai.Models;

namespace Benkyoukai.Services.Sessions;

public class SessionService : ISessionService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SessionService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task<bool> CreateSessionAsync(Session session)
    {
        return Task.FromResult(true);
    }
}
