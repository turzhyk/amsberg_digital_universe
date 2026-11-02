using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Amsberg_digital_universe.Domain.Common.Extensions;

public class GlobalExceptionHandler(IProblemDetailsService service) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var code = exception switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };
        var problemDetails = new ProblemDetails
        {
            Status = code,
            Detail = code != 500 ? exception?.Message : "Internal Server Error",
            Instance = context.Request.Path.Value,
        };
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
        return await service.TryWriteAsync(new ProblemDetailsContext()
            { HttpContext = context, ProblemDetails = problemDetails });
    }
}