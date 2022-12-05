using Benkyoukai.Api.Models;
using Benkyoukai.Api.RequestFeatures;

namespace Benkyoukai.Api.Repositories;

public interface ISessionRepository
{
    Task<bool> CreateSessionAsync(Session session);
    Task<Session?> GetSessionAsync(Guid id);
    Task<bool> UpsertSessionAsync(Session session);
    Task<bool> DeleteSessionAsync(Guid id);
    Task<(IEnumerable<Session>, MetaData)> GetSessionsAsync(SessionParameters sessionParameters);
    Task<bool> UserOwnsSessionAsync(Guid sessionId, string userId);
}
