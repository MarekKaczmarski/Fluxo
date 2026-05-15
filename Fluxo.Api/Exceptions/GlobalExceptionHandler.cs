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
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            ValidationException validationEx => CreateValidationProblemDetails(httpContext, validationEx),
            NotFoundException notFoundEx => CreateNotFoundProblemDetails(httpContext, notFoundEx),
            ConflictException conflictEx => CreateConflictProblemDetails(httpContext, conflictEx),
            DomainException domainEx => CreateBadRequestProblemDetails(httpContext, domainEx),
            _ => CreateInternalServerErrorProblemDetails(httpContext)
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

    private ProblemDetails CreateNotFoundProblemDetails(HttpContext context, NotFoundException ex)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Resource not found",
            Detail = ex.Message,
            Instance = context.Request.Path
        };
    }

    private ProblemDetails CreateBadRequestProblemDetails(HttpContext context, DomainException ex)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Domain rule violation",
            Detail = ex.Message,
            Instance = context.Request.Path
        };
    }

    private ProblemDetails CreateConflictProblemDetails(HttpContext context, ConflictException ex)
    {
        return new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            Title = "Conflict",
            Detail = ex.Message,
            Instance = context.Request.Path
        };
    }

    private ProblemDetails CreateInternalServerErrorProblemDetails(HttpContext context)
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
