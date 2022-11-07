namespace Benkyoukai.Contracts.Authentication;

public record AuthFailedResponse(IEnumerable<string> Errors);
