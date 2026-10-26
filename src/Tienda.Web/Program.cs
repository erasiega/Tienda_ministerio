using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Tienda.Web;
using Tienda.Web.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string direccionApi = builder.Configuration["Api:Base"]
    ?? throw new InvalidOperationException("Falta configurar Api:Base.");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(direccionApi, UriKind.Absolute) });
builder.Services.AddScoped<ClienteTienda>();
builder.Services.AddScoped<CarritoTienda>();

await builder.Build().RunAsync();
