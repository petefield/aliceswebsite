using AlicesWebsite.Shared.Accounts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace AlicesWebsite.Client
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider, IApiAuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public ApiAuthenticationStateProvider(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }
        public async Task<Token?> GetTokenAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            return string.IsNullOrEmpty(token)
                ? null
                : JsonSerializer.Deserialize<Token>(token);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();

            var identity = token is null || token.IsExpired
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ServiceExtensions.ParseClaimsFromJwt(token.Value), "jwt");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task<User> GetCurrentUser()
        { 
            var state = await GetAuthenticationStateAsync();

            foreach (var claim in state.User.Claims) { 
                Console.WriteLine(claim.Type);
                Console.WriteLine(claim.Value);
                Console.WriteLine("---");

            }

            var user = new User(
                userName: state.User.Claims.First(x => x.Type == "unique_name").Value,
                email: state.User.Claims.First(x => x.Type == "unique_name").Value,
                firstName: state.User.Claims.First(x => x.Type == "given_name").Value,
                lastName: state.User.Claims.First(x => x.Type == "family_name").Value);
            return user;
        }

        public async Task ClearToken() => await _jsRuntime.InvokeAsync<Token>("localStorage.removeItem", "authToken");

        public async Task SaveToken(Token token) => await _jsRuntime.InvokeAsync<Token>("localStorage.setItem", "authToken", JsonSerializer.Serialize(token));

        public async Task SetTokenAsync(Token? token)
        {
            await ((token == null || token.IsExpired)
                ? ClearToken()
                : SaveToken(token));

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
