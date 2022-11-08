using Benkyoukai.Contracts.Session;
using Benkyoukai.Api.Mappers;
using Benkyoukai.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Benkyoukai.Api.RequestFeatures;
using System.Text.Json;
using Benkyoukai.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Benkyoukai.Api.Extensions;

namespace Benkyoukai.Api.Controllers;

[Authorize(Roles = "User")]
[ApiController]
[Route("[controller]")]
public class SessionsController : ControllerBase
{
    private readonly ISessionRepository _sessionService;

    public SessionsController(ISessionRepository sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession(CreateSessionRequest request)
    {
        var session = new Session
        {
            Id = Guid.NewGuid(),
            UserId = HttpContext.GetUserId(),
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
        var session = await _sessionService.GetSessionAsync(id);
        if (session is null)
            return NotFound();

        return Ok(session.AsDto());
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
        var userOwnsSession = await _sessionService.UserOwnsSessionAsync(id, HttpContext.GetUserId());
        if (!userOwnsSession)
            return Forbid();

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
