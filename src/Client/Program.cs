using AlicesWebsite.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAuthorizationCore();

builder.Services.AddHttpClient<DefaultHttpClient>(c => {
    c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});

builder.Services.AddHttpClient<DefaultHttpClient>(c => {
    c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddTokenAuthenticationStateProvider();
await builder.Build().RunAsync();
