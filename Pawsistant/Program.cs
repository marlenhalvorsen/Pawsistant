using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pawsistant;
using Blazored.LocalStorage;
using Pawsistant.Service.Auth;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient for API calls
builder.Services.AddScoped(sp =>
    new HttpClient 
    { 
        BaseAddress = new Uri("https://localhost:7213"),

    });

// Register LocalStorage service
builder.Services.AddBlazoredLocalStorage();

// Register Custom Auth Provider
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthStateProvider>());

// Register the Auth service
builder.Services.AddScoped<IClientAuthService, ClientAuthService>();

// Register Blazor Authorization system
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
