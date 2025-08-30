using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EGeek.Api.Exceptions;

public class GlobalException(ILogger<GlobalException> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred.");

        ProblemDetails problemDetails = new()
        {
            Title = exception is ArgumentException ? "One or more arguments are invalid." : "Internal Server Error.",
            Detail = exception is ArgumentException ? exception.Message : "An unexpected error occurred. Please try again later.",
            Status = exception is ArgumentException ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError,
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
