using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Diagnostics;
using Ardalis.Result;

namespace AS_2025.Common;

public sealed class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var ex = exception.Demystify();

        _logger.LogError(ex, "An error occurred: {Message}", ex.Message);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = Result.Error(exception.ToStringDemystified());

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}