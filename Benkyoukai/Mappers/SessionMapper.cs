using Benkyoukai.Contracts.Session;
using Benkyoukai.Models;

namespace Benkyoukai.Mappers;

public static class SessionMapper
{
    public static SessionResponse AsDto(this Session session) => new(
        session.Id,
        session.Name,
        session.Description,
        session.StartDateTime,
        session.EndDateTime,
        session.LastModifiedDateTime);
}
