using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Extensions
{
   public static class HttpExtensions
   {
        public static async Task<T> GetFromJsonAsync<T>(this HttpClient client, string requestUri, HashSet<HttpStatusCode> codesToOverride = null)
        {
            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode && !IsHttpErrorCodeOverriden(codesToOverride, response.StatusCode))
                throw new HttpRequestException("Request to server was not successful");

            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static async Task<(string, HttpStatusCode)> GetPayloadWithHttpCodeAsync(this HttpClient client, string requestUri, HashSet<HttpStatusCode> codesToOverride = null)
        {
            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode && !IsHttpErrorCodeOverriden(codesToOverride, response.StatusCode))
                throw new HttpRequestException("Request to server was not successful");

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode && string.IsNullOrEmpty(result))
                throw new HttpRequestException("Request to server was not successful");

            return (result, response.StatusCode);
        }

        private static bool IsHttpErrorCodeOverriden(HashSet<HttpStatusCode> codesToOverride, HttpStatusCode code) => codesToOverride != null && codesToOverride.Contains(code);
    }
}
