using AlicesWebsite.Shared.Accounts;
using Microsoft.AspNetCore.Components.Authorization;

namespace AlicesWebsite.Client
{
    public interface IApiAuthenticationStateProvider
    {
        Task ClearToken();
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task<Token?> GetTokenAsync();
        Task SaveToken(Token token);
        Task SetTokenAsync(Token? token);
        Task<User> GetCurrentUser();
    }
}