namespace Benkyoukai.Contracts.Session;

public record SessionRequest(
    Guid Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    DateTime LastModifiedDateTime,
    List<string> Tags
);
