using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FluentValidation;
using System.Net;

namespace DotLink.Api.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400

                var errorResponse = new
                {
                    status = (int)HttpStatusCode.BadRequest,
                    title = "Validation error",
                    detail = "One or more parameters provided were not valid.",
                    errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                };

                context.Result = new JsonResult(errorResponse);
                context.ExceptionHandled = true;
            }
        }
    }
}