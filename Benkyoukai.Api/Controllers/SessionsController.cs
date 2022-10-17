using Benkyoukai.Contracts.Session;
using Benkyoukai.Api.Mappers;
using Benkyoukai.Api.Models;
using Benkyoukai.Api.ServiceErrors;
using Benkyoukai.Api.Services.Sessions;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Benkyoukai.Api.RequestFeatures;
using System.Text.Json;

namespace Benkyoukai.Api.Controllers;

public class SessionsController : ApiController
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
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            LastModifiedDateTime = DateTime.UtcNow
        };

        var created = await _sessionService.CreateSessionAsync(session);
        if (!created)
            return BadRequest();

        return CreatedAtAction(
            nameof(GetSession),
            new { id = session.Id },
            session.AsDto());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSession(Guid id)
    {
        var getSessionResult = await _sessionService.GetSessionAsync(id);

        return getSessionResult.Match(
            session => Ok(session.AsDto()),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpsertSession(Guid id, UpsertSessionRequest request)
    {
        var session = new Session
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            LastModifiedDateTime = DateTime.UtcNow
        };

        await _sessionService.UpsertSessionAsync(session);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSession(Guid id)
    {
        if (!await _sessionService.DeleteSessionAsync(id))
            return NotFound();
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetSessions([FromQuery] SessionParameters reqParams)
    {
        var (sessions, metaData) = await _sessionService.GetSessionsAsync(reqParams);

        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(metaData));
            
        return Ok(sessions);
    }
}
