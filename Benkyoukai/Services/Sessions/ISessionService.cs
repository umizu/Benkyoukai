using Benkyoukai.Models;

namespace Benkyoukai.Services.Sessions;

public interface ISessionService
{
    Task<bool> CreateSessionAsync(Session session);
}
