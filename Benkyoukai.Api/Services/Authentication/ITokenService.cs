using System.Security.Claims;
using Benkyoukai.Api.Models;

namespace Benkyoukai.Api.Services.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Creates new refresh token and sets it to the cookies.
    /// </summary>
    /// <returns>The refresh token.</returns>
    RefreshToken GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
