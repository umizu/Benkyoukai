using System.Data;
using Microsoft.Data.Sqlite;

namespace Benkyoukai.Data;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
