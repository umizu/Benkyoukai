using ErrorOr;

namespace Benkyoukai.ServiceErrors;

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
