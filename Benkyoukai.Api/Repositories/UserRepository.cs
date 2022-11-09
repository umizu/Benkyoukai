using Benkyoukai.Api.Data;
using Benkyoukai.Api.Models;
using Dapper;

namespace Benkyoukai.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var results = await connection.ExecuteAsync(
            @"INSERT INTO Users (Id, Username, Email, PasswordHash, PasswordSalt, VerificationToken)
            VALUES (@Id, @Username, @Email, @PasswordHash, @PasswordSalt, @VerificationToken)",
            user);

        return results > 0;
    }

    public async Task<User?> GetUserByConfirmationTokenAsync(string verificationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            @"SELECT * FROM Users 
            WHERE Verificationtoken = @VerificationToken",
                new { VerificationToken = verificationToken });
        return user;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM Users
            WHERE Username = @Username", new { Username = username });
    }

    public async Task<bool> UpdateRefreshTokenAsync(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var results = await connection.ExecuteAsync(
            @"UPDATE Users
            SET RefreshToken = @RefreshToken, TokenCreated = @TokenCreated, TokenExpires = @TokenExpires
            WHERE Id = @Id", user);
        return results > 0;
    }

    public async Task<bool> UpdateVerificationTokenAsync(Guid userId, string verificationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var results = await connection.ExecuteAsync(
            @"UPDATE Users
            SET VerificationToken = @VerificationToken
            WHERE Id = @Id", 
                new { Id = userId, VerificationToken = verificationToken });

        return results > 0;
    }

    public async Task<bool> UpdateVerifiedAsync(Guid userId, DateTime verifiedAt)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var results = await connection.ExecuteAsync(
            @"UPDATE Users
            SET VerifiedAt = @VerifiedAt
            WHERE Id = @Id", 
                new { Id = userId, VerifiedAt = verifiedAt });

        return results > 0;
    }
}
