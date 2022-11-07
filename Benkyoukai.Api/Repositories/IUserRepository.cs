using Benkyoukai.Api.Models;

namespace Benkyoukai.Api.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(User user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<bool> UpdateUserRefreshTokenAsync(User user);
}
