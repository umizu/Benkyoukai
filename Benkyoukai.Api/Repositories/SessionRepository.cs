using Benkyoukai.Api.Data;
using Benkyoukai.Api.Models;
using Benkyoukai.Api.RequestFeatures;
using Dapper;

namespace Benkyoukai.Api.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SessionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateSessionAsync(Session session)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(
            @"INSERT INTO Session (Id, UserId, Name, Description, StartDateTime, EndDateTime, LastModifiedDateTime) VALUES (@Id, @UserId, @Name, @Description, @StartDateTime, @EndDateTime, @LastModifiedDateTime)", session);

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

    public async Task<Session?> GetSessionAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        return await connection.QueryFirstOrDefaultAsync<Session>(
            @"SELECT * FROM Session WHERE Id = @Id", new { Id = id });
    }

    public async Task<(IEnumerable<Session>, MetaData)> GetSessionsAsync(SessionParameters sessionParameters)
    {
        var query =
            @"SELECT COUNT(Id)
            FROM session;

            SELECT *
            FROM session
            ORDER BY LastModifiedDateTime DESC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
        
        var skip = (sessionParameters.PageNumber - 1) * sessionParameters.PageSize;
        
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var multi = await connection.QueryMultipleAsync(query, new { Skip = skip, Take = sessionParameters.PageSize });

        var count = await multi.ReadSingleAsync<int>();
        var sessions = (await multi.ReadAsync<Session>()).ToList();

        var pagedSessions = new PagedList<Session>(sessions, count, sessionParameters.PageNumber, sessionParameters.PageSize);
        return (sessions, pagedSessions.MetaData);
    }

    public Task<bool> UpsertSessionAsync(Session session)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UserOwnsSessionAsync(Guid sessionId, string userId)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        var result = await connection.QueryFirstOrDefaultAsync<int>(
            @"SELECT COUNT(Id) FROM Session WHERE Id = @Id AND UserId = @UserId",
            new { Id = sessionId, UserId = userId });
    }
}
