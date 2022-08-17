using Dapper;

namespace Benkyoukai.Data;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
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
                StartDateTime TIMESTAMP NOT NULL,
                EndDateTime TIMESTAMP NOT NULL,
                LastModifiedDateTime TIMESTAMP NOT NULL)");
    }
}
