using FluentValidation;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var problemDetails = exception switch
        {
            ValidationException validationEx => CreateValidationProblemDetails(
                httpContext,
                validationEx
            ),
            NotFoundException notFoundEx => CreateProblemDetails(
                httpContext,
                StatusCodes.Status404NotFound,
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                "Resource not found",
                notFoundEx.Message
            ),
            ConflictException conflictEx => CreateProblemDetails(
                httpContext,
                StatusCodes.Status409Conflict,
                "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                "Conflict",
                conflictEx.Message
            ),
            DomainException domainEx => CreateProblemDetails(
                httpContext,
                StatusCodes.Status400BadRequest,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "Domain rule violation",
                domainEx.Message
            ),
            _ => CreateProblemDetails(
                httpContext,
                StatusCodes.Status500InternalServerError,
                "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                "Internal Server Error",
                "An unexpected error occurred on our side."
            ),
        };

        var status = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        if (status >= 500)
        {
            logger.LogError(
                exception,
                "Unhandled exception while processing {Method} {Path}",
                httpContext.Request.Method,
                httpContext.Request.Path
            );
        }
        else
        {
            logger.LogWarning(
                "Request {Method} {Path} failed with {Status}: {Message}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                status,
                exception.Message
            );
        }

        httpContext.Response.StatusCode = status;

        try
        {
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
        catch (Exception writeEx)
        {
            logger.LogError(
                writeEx,
                "Failed to write problem details response for {Path}",
                httpContext.Request.Path
            );
        }

        return true;
    }

    private static ProblemDetails CreateValidationProblemDetails(
        HttpContext context,
        ValidationException ex
    )
    {
        var problemDetails = CreateProblemDetails(
            context,
            StatusCodes.Status400BadRequest,
            "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            "Validation Error",
            "One or more validation errors occurred."
        );

        problemDetails.Extensions["errors"] = ex
            .Errors.GroupBy(x => x.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        int status,
        string type,
        string title,
        string detail
    ) =>
        new()
        {
            Status = status,
            Type = type,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path,
        };
}
