namespace Benkyoukai.Contracts.Session;

public record CreateSessionRequest(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    List<string> Tags
);
