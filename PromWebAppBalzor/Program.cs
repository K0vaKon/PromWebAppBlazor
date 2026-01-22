using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PromWebAppBalzor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Регистрируем ОДИН HttpClient, который смотрит на API друга
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:7188/") // Убедись, что у друга именно http, а не https
});

// ЗАПУСК должен быть в самом конце
await builder.Build().RunAsync();