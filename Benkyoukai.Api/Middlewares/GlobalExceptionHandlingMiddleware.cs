using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Benkyoukai.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problem = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Detail = "An internal server error occurred.",
                Status = context.Response.StatusCode,
                Type = "Server error"
            };

            var json = JsonSerializer.Serialize(problem);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
    }
}
