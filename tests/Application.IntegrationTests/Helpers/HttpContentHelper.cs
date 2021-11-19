using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.IntegrationTests.Helpers
{
    public static class HttpContentHelper
    {
        public static HttpContent ToJsonHttpContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            return httpContent;
        }

        public static async Task<T> ToResultAsync<T>(this HttpResponseMessage message)
        {
            string responseString = await message.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }


    }
}
