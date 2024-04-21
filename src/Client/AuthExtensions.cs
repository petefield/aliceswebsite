using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace AlicesWebsite.Client
{
    public static class ServiceExtensions
    {
        public static void AddTokenAuthenticationStateProvider(this IServiceCollection services)
        {
            services.AddScoped<IApiAuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<ApiAuthenticationStateProvider>( p => (ApiAuthenticationStateProvider)p.GetRequiredService<IApiAuthenticationStateProvider>());
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ApiAuthenticationStateProvider>());
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? "")) ?? new List<Claim>();
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}