using AlicesWebsite.Shared;
using OneOf;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Linq;

namespace AlicesWebsite.Client
{
    public class DefaultHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IApiAuthenticationStateProvider _apiAuthenticationStateProvider;

        public DefaultHttpClient(HttpClient client, IApiAuthenticationStateProvider apiAuthenticationStateProvider)
        {
            if (client == null) throw new ArgumentNullException("client");
            _httpClient = client;
            _apiAuthenticationStateProvider = apiAuthenticationStateProvider;
        }

        public async Task<OneOf<TResponse, HttpResponseMessage>> Post<TRequest, TResponse>(string url, TRequest request) where TResponse : class
        {
            if (string.IsNullOrEmpty(_httpClient.DefaultRequestHeaders.Authorization?.Parameter))
            { 
                var token = await _apiAuthenticationStateProvider.GetTokenAsync();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token!.Value}");
            }

            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                var result = await Deserialise<TResponse>(response);
                if (result is not null)
                    return result;
            }

            return response;
        }

        private static async Task<T?> Deserialise<T>(HttpResponseMessage responseMessage) where T : class
        {
            var stringContent = await responseMessage.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return result;
        }
        public async Task<T?> Get<T>(string url) where T : class 
        {
            return await _httpClient.GetFromJsonAsync<T>(url);
        }
    }
}
