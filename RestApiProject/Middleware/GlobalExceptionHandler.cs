using Microsoft.AspNetCore.Mvc;
using RestApiProject.Exceptions;
using System.Text.Json;

namespace RestApiProject.Middleware;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error"
        };

        switch (exception)
        {
            case NotFoundException notFoundEx:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Not Found";
                problemDetails.Detail = notFoundEx.Message;
                break;

            case ValidationException validationEx:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Error";
                problemDetails.Extensions["errors"] = validationEx.Errors;
                break;

            default:
                problemDetails.Detail = "An unexpected error occurred.";
                break;
        }

        context.Response.StatusCode = problemDetails.Status.Value;

        var json = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(json);
    }
}