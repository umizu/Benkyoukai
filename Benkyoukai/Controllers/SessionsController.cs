using Benkyoukai.Contracts.Session;
using Microsoft.AspNetCore.Mvc;

namespace Benkyoukai.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateSession(CreateSessionRequest request)
    {
        return Ok(request);
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
