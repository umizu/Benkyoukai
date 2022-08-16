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

        var created = await _sessionService.CreateSessionAsync(session);
        if (!created)
            return BadRequest();

        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSession(int id)
    {
        var session = await _sessionService.GetSessionAsync(id);
        if (session is null)
            return NotFound();

        return Ok(session);
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
