using System.Security.Claims;
using Benkyoukai.Api.Models;

namespace Benkyoukai.Api.Extensions;

public static class GeneralExtensions
{
    public static string GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User is null)
            return string.Empty;

        return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Name).Value;
    }

    public static bool IsVerified (this User user) => user.VerifiedAt is not null;
}
