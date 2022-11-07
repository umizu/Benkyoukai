
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Benkyoukai.Api.Extensions;
using Benkyoukai.Api.Models;
using Benkyoukai.Api.Repositories;
using Benkyoukai.Contracts.Authentication;

namespace Benkyoukai.Api.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpCtxAccessor;

    public AuthService(IUserRepository userRepo, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _httpCtxAccessor = httpContextAccessor;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepo.GetUserByUsernameAsync(request.Username) is not null)
            return new AuthResult(IsSuccess: false) { Errors = new[] { "Username is already taken." } };

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        if (!await _userRepo.CreateUserAsync(newUser))
            return new AuthResult(IsSuccess: false)
            { Errors = new[] { "Failed to create user." } };

        var claims = GenerateClaims(newUser);
        string token = _tokenService.GenerateAccessToken(claims);

        var refreshToken = _tokenService.GenerateRefreshToken();

        newUser.RefreshToken = refreshToken.Token;
        newUser.TokenCreated = refreshToken.Created;
        newUser.TokenExpires = refreshToken.Expires;

        if (!await _userRepo.UpdateUserRefreshTokenAsync(newUser))
            throw new Exception("Failed to update user's refresh token.");

        return new AuthResult(IsSuccess: true)
        {
            TokenType = "Bearer",
            AccessToken = token,
            ExpiresIn = "3600",
            RefreshToken = refreshToken.Token
        };
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {

        var user = await _userRepo.GetUserByUsernameAsync(request.Username);
        if (user is null)
            return new AuthResult(IsSuccess: false)
            { Errors = new[] { "Username or password is incorrect." } };
            
        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return new AuthResult(IsSuccess: false)
            { Errors = new[] { "Username or password is incorrect." } };

        var claims = GenerateClaims(user);
        string token = _tokenService.GenerateAccessToken(claims);

        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken.Token;
        user.TokenCreated = refreshToken.Created;
        user.TokenExpires = refreshToken.Expires;

        if (!await _userRepo.UpdateUserRefreshTokenAsync(user))
            throw new Exception("Failed to update user's refresh token.");

        return new AuthResult(IsSuccess: true)
        {
            TokenType = "Bearer",
            AccessToken = token,
            ExpiresIn = "3600",
            RefreshToken = refreshToken.Token
        };
    }

    private static IEnumerable<Claim> GenerateClaims(User user)
        => new List<Claim>()
            {
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Role, "User")
            };

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }

    public async Task<AuthResult> RefreshTokenAsync()
    {
        var refreshToken = _httpCtxAccessor.HttpContext!.Request.Cookies["refreshToken"];
        var expiredJwt = _httpCtxAccessor.HttpContext!.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(refreshToken)
            || string.IsNullOrEmpty(expiredJwt)
            || _tokenService.GetPrincipalFromExpiredToken(expiredJwt) is not ClaimsPrincipal principal
            || principal.Identity?.Name is not string username)
            return new AuthResult(IsSuccess: false) { Errors = new string[] { "Invalid token." } };

        var user = await _userRepo.GetUserByUsernameAsync(username);
        if (user is null || user.RefreshToken != refreshToken || user.TokenExpires < DateTime.Now)
            return new AuthResult(IsSuccess: false) { Errors = new string[] { "Invalid token." } };

        var claims = GenerateClaims(user);
        string token = _tokenService.GenerateAccessToken(claims);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;

        if (!await _userRepo.UpdateUserRefreshTokenAsync(user))
            throw new Exception("Failed to update user's refresh token.");

        return new AuthResult(IsSuccess: true)
        {
            TokenType = "Bearer",
            AccessToken = token,
            ExpiresIn = "3600",
            RefreshToken = newRefreshToken.Token
        };
    }

    public async Task<bool> RevokeAsync()
    {
        var username = _httpCtxAccessor.HttpContext!.GetUserId();
        if (username is null)
            return await Task.FromResult(false);

        var user = await _userRepo.GetUserByUsernameAsync(username);
        if (user is null)
            return false;

        user.RefreshToken = null;
        user.TokenCreated = null;
        user.TokenExpires = null;

        if (!await _userRepo.UpdateUserRefreshTokenAsync(user))
            throw new Exception("Failed to update user's refresh token.");

        return true;
    }
}
