using Benkyoukai.Contracts.Authentication;

namespace Benkyoukai.Api.Services.Authentication;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task<AuthResult> LoginAsync(LoginRequest request);
    Task<AuthResult> RefreshTokenAsync();
    Task<bool> ConfirmEmailAsync(string token, string email);
    Task<bool> RevokeAsync();
}
