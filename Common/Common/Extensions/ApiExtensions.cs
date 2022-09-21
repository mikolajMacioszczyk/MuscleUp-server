using Microsoft.AspNetCore.Http;

namespace Common.Extensions
{
    public static class ApiExtensions
    {
        public static string GetUserAgent(this HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.Request.Headers.ContainsKey(CommonConsts.UserAgent) ? context.Request.Headers[CommonConsts.UserAgent].FirstOrDefault() : null;
        }
    }
}
