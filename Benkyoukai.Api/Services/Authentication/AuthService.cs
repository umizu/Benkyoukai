
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Benkyoukai.Api.Extensions;
using Benkyoukai.Api.Models;
using Benkyoukai.Api.Repositories;
using Benkyoukai.Api.Services.Email;
using Benkyoukai.Contracts.Authentication;
using Benkyoukai.Services.Contracts.Email;

namespace Benkyoukai.Api.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpCtxAccessor;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepo, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IEmailService emailService, ILogger<AuthService> logger)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _httpCtxAccessor = httpContextAccessor;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepo.GetUserByUsernameAsync(request.Username) is not null)
            return new AuthResult(IsSuccess: false) { Errors = new[] { "Username is already taken." } };

        // todo - check if email exists

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var token = _tokenService.GenerateEmailConfirmationToken();

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            VerificationToken = token
        };

        if (!await _userRepo.CreateUserAsync(newUser))
            return new AuthResult(IsSuccess: false)
            { Errors = new[] { "Failed to create user." } };

        var callbackUrl = _httpCtxAccessor.HttpContext!.Request.Scheme + "://" + _httpCtxAccessor.HttpContext.Request.Host + "/auth/confirm-email?token=" + token + "&email=" + newUser.Email;

        callbackUrl = callbackUrl.Replace("+", "%2B");
        callbackUrl = HtmlEncoder.Default.Encode(callbackUrl);

        var emailBody = $@"<p>Thank you for registering with Benkyoukai!</p>
            <p>Please confirm your email by clicking the link below:</p>
            <a href=""{callbackUrl}"">Confirm Email</a>";

        _emailService.SendRegistrationEmail(
            new EmailRegisterMessageDto(
                Address: newUser.Email,
                Subject: "Benkyoukai - Confirm Email",
                Body: emailBody));

        return new AuthResult(IsSuccess: true);
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {

        var user = await _userRepo.GetUserByUsernameAsync(request.Username);
        if (user is null)
            return new AuthResult(IsSuccess: false)
            { Errors = new[] { "Username or password is incorrect." } };

        if (!user.IsVerified())
            return new AuthResult(IsSuccess: false)
            { Errors = new[] { "Please confirm your email address." } };

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return new AuthResult(IsSuccess: false)
            { Errors = new[] { "Username or password is incorrect." } };

        var claims = GenerateClaims(user);
        string token = _tokenService.GenerateAccessToken(claims);

        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken.Token;
        user.TokenCreated = refreshToken.Created;
        user.TokenExpires = refreshToken.Expires;

        if (!await _userRepo.UpdateRefreshTokenAsync(user))
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

        if (!await _userRepo.UpdateRefreshTokenAsync(user))
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

        if (!await _userRepo.UpdateRefreshTokenAsync(user))
            throw new Exception("Failed to update user's refresh token.");

        return true;
    }

    public async Task<bool> ConfirmEmailAsync(string token, string email)
    {
        var user = await _userRepo.GetUserByConfirmationTokenAsync(token);
        if (user is null)
            return false;
        if (user.Email != email)
            return false;

        user.VerifiedAt = DateTime.Now;
        if (!await _userRepo.UpdateVerifiedAsync(user.Id, user.VerifiedAt.Value))
            throw new Exception("Unable to update verified user.");

        return true;
    }
}
