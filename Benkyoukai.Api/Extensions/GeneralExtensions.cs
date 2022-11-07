using System.Security.Claims;

namespace Benkyoukai.Api.Extensions;

public static class GeneralExtensions
{
    public static string GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User is null)
            return string.Empty;

        return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Name).Value;
    }
}
