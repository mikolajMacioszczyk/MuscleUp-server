using Newtonsoft.Json;

namespace CarnetsTests
{
    public static class ObjectExtensions
    {
        public static string AsJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
