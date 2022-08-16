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
            @"INSERT INTO Session (Name, Description, StartDateTime, EndDateTime, LastModifiedDateTime) VALUES (@Name, @Description, @StartDateTime, @EndDateTime, @LastModifiedDateTime)", session);

        return result > 0;
    }

    public async Task<Session?> GetSessionAsync(int id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<Session>(
            @"SELECT * FROM Session WHERE Id = @Id", new { Id = id });
    }
}
