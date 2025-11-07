using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotLink.Api.Filters
{
    public class UnauthorizedAccessExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden; // 403

                var errorResponse = new
                {
                    status = (int)HttpStatusCode.Forbidden,
                    title = "Access denied",
                    detail = context.Exception.Message
                };

                context.Result = new JsonResult(errorResponse);
                context.ExceptionHandled = true;
            }
        }
    }
}
