using BlazorComponentBus;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GlobalAzure;
using GlobalAzure.Helpers;
using GlobalAzure.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddHttpClient<IAggregateService, AggregateService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["GlobalAzure:Muflone:MufloneApiUri"]!);
})
    .AddPolicyHandler(PolicyHelper.GetRetryPolicy());

builder.Services.AddMudServices();
builder.Services.AddScoped<ComponentBus>();

await builder.Build().RunAsync();