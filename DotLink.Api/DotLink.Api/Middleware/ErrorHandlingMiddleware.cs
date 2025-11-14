using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using FluentValidation;
using DotLink.Application.Exceptions;

namespace DotLink.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var title = "Server Error";
            var detail = "An unexpected error occurred.";

            switch (exception)
            {
                case DotLinkValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    title = "Validation Error";
                    break;

                case DotLinkNotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound; // 404
                    title = "Resource Not Found";
                    detail = notFoundException.Message;
                    break;

                case DotLinkUnauthorizedAccessException unauthorizedAccessException:
                    statusCode = HttpStatusCode.Forbidden; // 403
                    title = "Access Denied";
                    detail = unauthorizedAccessException.Message;
                    break;

            }

            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new
            {
                status = (int)statusCode,
                title = title,
                detail = detail
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}