using Microsoft.AspNetCore.Mvc;

namespace Benkyoukai.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error() => Problem();
}
