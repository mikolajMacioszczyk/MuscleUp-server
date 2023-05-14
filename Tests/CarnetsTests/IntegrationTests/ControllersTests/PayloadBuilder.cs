using Newtonsoft.Json;
using System.Text;

namespace CarnetsTests.IntegrationTests.ControllersTests
{
    public static class PayloadBuilder
    {
        public static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
