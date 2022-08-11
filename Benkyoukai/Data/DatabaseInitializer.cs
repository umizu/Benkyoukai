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
            CREATE TABLE IF NOT EXISTS Session(
                Id TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Description TEXT,
                StartDateTime TEXT NOT NULL,
                EndDateTime TEXT NOT NULL,
                LastDateModified TEXT NOT NULL)");
    }
}
