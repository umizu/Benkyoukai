using Benkyoukai.Contracts.Session;
using Benkyoukai.Models;
using Benkyoukai.Services.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Benkyoukai.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession(CreateSessionRequest request)
    {
        var session = new Session
        {
            Name = request.Name,
            Description = request.Description,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            LastModifiedDateTime = DateTime.UtcNow
        };

    // todo: save to database
    await _sessionService.CreateSessionAsync(session);

        var response = new SessionResponse(
            session.Id,
            session.Name,
            session.Description,
            session.StartDateTime,
            session.EndDateTime,
            session.LastModifiedDateTime);

        return CreatedAtAction(
            actionName: nameof(GetSession),
            routeValues: new { id = session.Id
},
            value: response);
    }

    [HttpGet("{id:guid}")]
public IActionResult GetSession(Guid id)
{
    return Ok(id);
}

[HttpPut("{id:guid}")]
public IActionResult UpsertSession(Guid id, UpsertSessionRequest request)
{
    return Ok(request);
}

[HttpDelete("{id:guid}")]
public IActionResult DeleteSession(Guid id)
{
    return Ok(id);
}
}
