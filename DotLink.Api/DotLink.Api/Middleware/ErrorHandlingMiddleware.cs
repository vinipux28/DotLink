using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using FluentValidation;
using DotLink.Application.Exceptions;
using System.Linq;

namespace DotLink.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log only unexpected server errors (those that will produce HTTP 500)
                if (ex is not DotLinkValidationException
                    && ex is not DotLinkNotFoundException
                    && ex is not DotLinkUnauthorizedAccessException
                    && ex is not DotLinkConflictException)
                {
                    _logger.LogError(ex, "An unexpected error occurred while processing the request.");
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var title = "Server Error";
            var detail = "An unexpected error occurred.";
            IDictionary<string, string[]>? errors = null;

            switch (exception)
            {
                case DotLinkValidationException validationException when validationException.ValidationErrors != null:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Validation Error";
                    detail = "One or more validation errors occurred.";
                    errors = validationException.ValidationErrors
                        .Where(e => e != null)
                        .GroupBy(e => e!.PropertyName ?? string.Empty)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e!.ErrorMessage ?? string.Empty).ToArray()
                        );
                    break;

                case DotLinkValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Validation Error";
                    detail = "One or more validation errors occurred.";
                    errors = new Dictionary<string, string[]>();
                    break;

                case DotLinkNotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound; // 404
                    title = "Resource Not Found";
                    detail = notFoundException.Message;
                    break;

                case DotLinkUnauthorizedAccessException unauthorizedAccessException:
                    // Map to 401 Unauthorized for authentication failures (e.g., invalid credentials)
                    statusCode = HttpStatusCode.Unauthorized; // 401
                    title = "Unauthorized";
                    detail = unauthorizedAccessException.Message;
                    break;

                case DotLinkConflictException conflictException:
                    statusCode = HttpStatusCode.Conflict;
                    title = "Resource Conflict";
                    detail = conflictException.Message;
                    break;

                default:
                    // For unhandled exceptions, keep the default 500 status code
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new
            {
                status = (int)statusCode,
                title = title,
                detail = detail,
                errors = errors
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}