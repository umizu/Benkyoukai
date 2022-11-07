namespace Benkyoukai.Api.Models;

public class RefreshToken
{
    public string Token { get; init; } = default!;
    public DateTime Created { get; init; } = DateTime.Now;
    public DateTime Expires { get; init; }
}
