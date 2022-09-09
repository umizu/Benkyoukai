using Microsoft.AspNetCore.Mvc;

namespace Benkyoukai.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error() => Problem();
}
