

using Microsoft.AspNetCore.Mvc;

namespace Benkyoukai.Api.Controllers;

[ApiController]
public class TestsController : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get() => await Task.FromResult(Ok("Hello From TestsController"));
}
