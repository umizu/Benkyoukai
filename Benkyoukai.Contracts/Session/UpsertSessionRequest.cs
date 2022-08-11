namespace Benkyoukai.Contracts.Session;

public record UpsertSessionRequest(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime);
