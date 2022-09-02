using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Common.Helpers
{
    public class DeveloperExceptionMiddleware : ExceptionMiddlewareBase
    {
        public DeveloperExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddlewareBase> logger) : base(next, logger)
        {
        }

        protected override void BuildResponseContent(Exception ex, HttpContext context, ref StringBuilder sb)
        {
            var type = ex.GetType();

            sb.AppendLine($"The {type.Name} has been thrown.");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine(ex.StackTrace);
        }
    }
}
