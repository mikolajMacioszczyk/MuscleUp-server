using System.Web;

namespace Common.Helpers
{
    public static class UriHelper
    {
        public static string AppendQueryParamToUri(this string uri, string key, string value)
        {
            var uriBuilder = new UriBuilder(uri);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[key] = value;
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }
    }
}
