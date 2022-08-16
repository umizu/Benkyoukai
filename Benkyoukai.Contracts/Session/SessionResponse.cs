namespace Benkyoukai.Contracts.Session;

public record SessionResponse(
    int Id,
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    DateTime LastModifiedDateTime);
