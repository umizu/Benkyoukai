using Dapper;

namespace Benkyoukai.Api.Data;

public class DbInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(IDbConnectionFactory connectionFactory, ILogger<DbInitializer> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(
                @"CREATE TABLE IF NOT EXISTS Users (
                Id UUID PRIMARY KEY,
                Username VARCHAR(30) NOT NULL,
                PasswordHash BYTEA NOT NULL,
                PasswordSalt BYTEA NOT NULL,
                Email VARCHAR(255) NOT NULL,
                RefreshToken VARCHAR(255),
                TokenCreated TIMESTAMP,
                TokenExpires TIMESTAMP,
                VerificationToken VARCHAR NOT NULL,
                VerifiedAt TIMESTAMP)");

        await connection.ExecuteAsync(
                @"CREATE TABLE IF NOT EXISTS Session (
                Id UUID PRIMARY KEY,
                UserId VARCHAR(255) NOT NULL,
                Name VARCHAR NOT NULL,
                Description VARCHAR NOT NULL,
                StartDateTime DATE NOT NULL,
                EndDateTime DATE NOT NULL,
                LastModifiedDateTime DATE NOT NULL)");

        _logger.LogInformation("Database initialized");
    }
}
