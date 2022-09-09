using Benkyoukai.Data;
using Benkyoukai.Models;
using Benkyoukai.ServiceErrors;
using Dapper;
using ErrorOr;

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

    public async Task<bool> DeleteSessionAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"DELETE FROM Session WHERE Id = @Id",
            new { Id = id });
        return result > 0;
    }

    public async Task<ErrorOr<Session>> GetSessionAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var session = await connection.QueryFirstOrDefaultAsync<Session>(
            @"SELECT * FROM Session WHERE Id = @Id", new { Id = id });

        if (session is null)
            return Errors.Session.NotFound;
        return session;
    }

    public Task<bool> UpsertSessionAsync(Session session)
    {
        throw new NotImplementedException();
    }
}
