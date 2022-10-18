using Dapper;

namespace Benkyoukai.Api.Data;

public class SqliteDbInitilizer : IDbInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SqliteDbInitilizer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS Session (
                Id UUID PRIMARY KEY,
                Name TEXT NOT NULL,
                Description TEXT NOT NULL,
                StartDateTime DATE NOT NULL,
                EndDateTime DATE NOT NULL,
                LastModifiedDateTime DATE NOT NULL)");
    }
}
