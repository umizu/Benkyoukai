using Benkyoukai.Api.Models;

namespace Benkyoukai.Api.Repositories;

public interface IUserRepository
{
    Task<bool> CreateUserAsync(User user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByConfirmationTokenAsync(string verificationToken);
    Task<bool> UpdateRefreshTokenAsync(User user);
    
    Task<bool> UpdateVerificationTokenAsync(Guid userId, string verificationToken);
    Task<bool> UpdateVerifiedAsync(Guid userId, DateTime verifiedAt);
}
