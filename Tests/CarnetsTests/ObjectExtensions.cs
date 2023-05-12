using System.Text.Json;

namespace CarnetsTests
{
    public static class ObjectExtensions
    {
        public static string AsJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
