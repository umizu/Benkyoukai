using Benkyoukai.Api.Services.Authentication;
using Benkyoukai.Contracts.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Benkyoukai.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _validator;

    public AuthController(IAuthService authService, IValidator<RegisterRequest> validator)
    {
        _authService = authService;
        _validator = validator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(
                new AuthFailedResponse(
                    Errors: validationResult.Errors.Select(x => x.ErrorMessage)));

        var authResult = await _authService.RegisterAsync(request);

        if (!authResult.IsSuccess)
            return BadRequest(new AuthFailedResponse(
                Errors: authResult.Errors));

        return Ok(new AuthSuccessResponse(
            TokenType: authResult.TokenType,
            AccessToken: authResult.AccessToken,
            ExpiresIn: authResult.ExpiresIn,
            RefreshToken: authResult.RefreshToken));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var authResult = await _authService.LoginAsync(request);

        if (!authResult.IsSuccess)
            return BadRequest(new AuthFailedResponse(
                Errors: authResult.Errors));

        return Ok(new AuthSuccessResponse(
            TokenType: authResult.TokenType,
            AccessToken: authResult.AccessToken,
            ExpiresIn: authResult.ExpiresIn,
            RefreshToken: authResult.RefreshToken));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var authResult = await _authService.RefreshTokenAsync();

        if (!authResult.IsSuccess)
            return BadRequest(new AuthFailedResponse(
                Errors: authResult.Errors));

        return Ok(new AuthSuccessResponse(
            TokenType: authResult.TokenType,
            AccessToken: authResult.AccessToken,
            ExpiresIn: authResult.ExpiresIn,
            RefreshToken: authResult.RefreshToken));
    }

    [Authorize()]
    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke()
    {
        var revoked = await _authService.RevokeAsync();
        if (!revoked)
            return BadRequest(new AuthFailedResponse(
                Errors: new[] { "Failed to revoke token." }));
        return NoContent();
    }
}
