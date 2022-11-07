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
            @"INSERT INTO Users (Id, Username, PasswordHash, PasswordSalt, Email)
            VALUES (@Id, @Username, @PasswordHash, @PasswordSalt, @Email)",
            user);

        return results > 0;
    }

    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM Users
            WHERE Username = @Username", new { Username = username });
    }

    public async Task<bool> UpdateUserRefreshTokenAsync(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var results = await connection.ExecuteAsync(
            @"UPDATE Users
            SET RefreshToken = @RefreshToken, TokenCreated = @TokenCreated, TokenExpires = @TokenExpires
            WHERE Id = @Id", user);
        return results > 0;
    }
}
