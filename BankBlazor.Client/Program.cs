using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BankBlazor.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Koppla <App> till <div id="app"> i index.html
builder.RootComponents.Add<App>("#app");

// L�gg ocks� till HeadOutlet s� att <PageTitle> fungerar
builder.RootComponents.Add<HeadOutlet>("head::after");

// Peka HttpClient mot ditt API p� r�tt port (och inte mot klientens egen 5001)
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("https://localhost:7252/") }
);

await builder.Build().RunAsync();
