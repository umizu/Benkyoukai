namespace Benkyoukai.Api.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? RefreshToken { get; set; }
    public DateTime? TokenCreated { get; set; }
    public DateTime? TokenExpires { get; set; }

    // public string? VerificationToken { get; set; }
    // public DateTime? VerifiedAt { get; set; }
    // public string? PasswordResetToken { get; set; }
    // public DateTime? ResetTokenExpires { get; set; }


}
