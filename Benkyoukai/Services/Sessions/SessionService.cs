using Benkyoukai.Data;
using Benkyoukai.Models;
using Dapper;

namespace Benkyoukai.Services.Sessions;

public class SessionService : ISessionService
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SessionService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateSessionAsync(Session session)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(
            @"INSERT INTO Session (Id, Name, Description, StartDateTime, EndDateTime, LastModifiedDateTime) VALUES (@Id, @Name, @Description, @StartDateTime, @EndDateTime, @LastModifiedDateTime)", session);

        return result > 0;
    }
}
