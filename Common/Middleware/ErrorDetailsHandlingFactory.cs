using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace ecommerse_api.Common.Middleware;

public class ErrorHandlingDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;

    public ErrorHandlingDetailsFactory(
        IOptions<ApiBehaviorOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        statusCode ??= StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance,
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(modelStateDictionary);

        statusCode ??= StatusCodes.Status400BadRequest;

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance,
        };

        if (title != null)
        {
            // For validation problem details, don't overwrite the default title with null.
            problemDetails.Title = title;
        }

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        problemDetails.Extensions.Add("requestId", httpContext?.TraceIdentifier);

        // Add human-readable error message and error type description
        problemDetails.Extensions.Add("message", GetMessageForStatusCode(statusCode));
        problemDetails.Extensions.Add("errorType", GetErrorTypeForStatusCode(statusCode));
    }
    
    // Method to get human-readable error message for a given status code
        private static string GetMessageForStatusCode(int statusCode)
        {
            // You can implement custom logic here to provide meaningful error messages based on status codes
            switch (statusCode)
            {
                case StatusCodes.Status400BadRequest:
                    return "Bad request. Please check your request and try again.";
                case StatusCodes.Status401Unauthorized:
                    return "Unauthorized. Please provide valid credentials.";
                case StatusCodes.Status403Forbidden:
                    return "Forbidden. You do not have permission to access this resource.";
                case StatusCodes.Status404NotFound:
                    return "Resource not found. The requested resource does not exist.";
                case StatusCodes.Status500InternalServerError:
                    return "Internal server error. Please try again later.";
                default:
                    return "An error occurred. Please try again or contact support.";
            }
        }

        // Method to get error type description for a given status code
        private static string GetErrorTypeForStatusCode(int statusCode)
        {
            // You can implement custom logic here to provide error type descriptions based on status codes
            switch (statusCode)
            {
                case StatusCodes.Status400BadRequest:
                    return "Client Error";
                case StatusCodes.Status401Unauthorized:
                    return "Authentication Error";
                case StatusCodes.Status403Forbidden:
                    return "Authorization Error";
                case StatusCodes.Status404NotFound:
                    return "Resource Not Found Error";
                case StatusCodes.Status500InternalServerError:
                    return "Internal Server Error";
                default:
                    return "Unknown Error";
            }
        }
}