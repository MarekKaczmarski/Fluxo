using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            ValidationException validationEx => CreateValidationProblemDetails(httpContext, validationEx),
            _ => CreateInternalServerErrorProblemDetails(httpContext, exception)
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private ProblemDetails CreateValidationProblemDetails(HttpContext context, ValidationException ex)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Validation Error",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };

        problemDetails.Extensions["errors"] = ex.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        return problemDetails;
    }

    private ProblemDetails CreateInternalServerErrorProblemDetails(HttpContext context, Exception ex)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Detail = "An unexpected error occurred on our side.",
            Instance = context.Request.Path
        };
    }
}