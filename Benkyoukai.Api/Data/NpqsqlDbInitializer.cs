using Dapper;

namespace Benkyoukai.Api.Data;

public class NpqsqlDbInitializer : IDbInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public NpqsqlDbInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS Session (
                Id UUID PRIMARY KEY,
                Name VARCHAR NOT NULL,
                Description VARCHAR NOT NULL,
                StartDateTime DATE NOT NULL,
                EndDateTime DATE NOT NULL,
                LastModifiedDateTime DATE NOT NULL)");
    }
}
