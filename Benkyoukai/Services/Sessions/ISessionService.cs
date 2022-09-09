using Benkyoukai.Models;
using ErrorOr;

namespace Benkyoukai.Services.Sessions;

public interface ISessionService
{
    Task<bool> CreateSessionAsync(Session session);
    Task<ErrorOr<Session>> GetSessionAsync(Guid id);
    Task<bool> UpsertSessionAsync(Session session);
    Task<bool> DeleteSessionAsync(Guid id);
}
