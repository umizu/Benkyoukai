namespace Benkyoukai.Api.Services.Authentication;

public class AuthResult
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = default!;
    public string TokenType { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public string ExpiresIn { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public AuthResult(bool IsSuccess)
    {
        this.IsSuccess = IsSuccess;
    }
}
