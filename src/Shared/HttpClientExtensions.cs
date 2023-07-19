using OneOf;
using OneOf.Types;
using System.Net.Http.Json;
using System.Text.Json;

namespace AlicesWebsite.Shared
{
    public static class HttpClientExtensions
    {
        public static async Task<OneOf<TResponse, HttpResponseMessage>> Post<TRequest, TResponse>(this HttpClient httpClient, string url, TRequest request) where TResponse : class
        {
            var response = await httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var result = await Deserialise<TResponse>(response);
                if (result is not null)
                    return result;
            }

            return response;
        }

        private static async Task<T?> Deserialise<T>(this HttpResponseMessage responseMessage) where T : class 
        {
            var stringContent = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return result;
        }

    }
}
