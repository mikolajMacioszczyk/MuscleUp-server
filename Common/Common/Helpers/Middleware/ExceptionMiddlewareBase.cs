using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace Common.Helpers
{
    public abstract class ExceptionMiddlewareBase
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddlewareBase> _logger;

        public ExceptionMiddlewareBase(RequestDelegate next, ILogger<ExceptionMiddlewareBase> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (InvalidInputException ex)
            {
                await HandleBadRequestExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        protected async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            _logger.LogWarning(exception.Message);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var sb = new StringBuilder();

            BuildResponseContent(exception, context, ref sb);

            await context.Response.WriteAsync(sb.ToString());
        }

        protected async Task HandleBadRequestExceptionAsync(HttpContext context, InvalidInputException exception)
        {
            context.Response.ContentType = "application/json";
            _logger.LogInformation(exception.Message);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var sb = new StringBuilder();

            BuildResponseContent(exception, context, ref sb);

            await context.Response.WriteAsync(sb.ToString());
        }

        protected abstract void BuildResponseContent(Exception ex, HttpContext context, ref StringBuilder sb);
    }
}
