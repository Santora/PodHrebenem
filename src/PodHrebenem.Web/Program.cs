using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PodHrebenem.Shared;
using PodHrebenem.Shared.Services;
using PodHrebenem.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IPlatformService, WebPlatformService>();
builder.Services.AddScoped<IPreferenceStore, LocalStoragePreferenceStore>();
builder.Services.AddPodHrebenemShared();

await builder.Build().RunAsync();
