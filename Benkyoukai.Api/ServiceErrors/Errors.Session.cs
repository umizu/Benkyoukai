using ErrorOr;

namespace Benkyoukai.Api.ServiceErrors;

public static class Errors
{
    public static class Session
    {
        public static Error NotFound => Error.NotFound(
            code: "Session.NotFound",
            description: "Session not found"
        );
    }
}
