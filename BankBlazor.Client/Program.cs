using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BankBlazor.Client;
namespace BankBlazor.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });


        await builder.Build().RunAsync();
    }
}
