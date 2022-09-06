using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Common.Resolvers
{
    public class IgnoreJsonPropertyAttributeContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.PropertyName = ToCamelCase(property.UnderlyingName);

            return property;
        }

        private static string ToCamelCase(string str) => System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(str);
    }
}
