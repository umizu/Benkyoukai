using System.Text.Json.Serialization;

namespace Benkyoukai.Contracts.Authentication;

public record AuthSuccessResponse(
    [property: JsonPropertyName("token_type")] string TokenType,
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("expires_in")] string ExpiresIn,
    [property: JsonPropertyName("refresh_token")] string RefreshToken);
