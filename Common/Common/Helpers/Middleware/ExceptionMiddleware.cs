using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Common.Helpers
{
    public class ExceptionMiddleware : ExceptionMiddlewareBase
    {
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddlewareBase> logger) : base(next, logger)
        {
        }

        protected override void BuildResponseContent(Exception ex, HttpContext context, ref StringBuilder sb)
        {
            sb.AppendLine($"Something went wrong: {ex.Message}");
        }
    }
}
