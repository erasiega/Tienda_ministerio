using Tienda.Api.Endpoints;
using Tienda.Api.Errores;
using Tienda.Core.Servicios;
using Tienda.Datos;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AgregarDatos(builder.Configuration["BaseDeDatos:Motor"], builder.Configuration["BaseDeDatos:Conexion"]);
builder.Services.AddScoped<ServicioCatalogo>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorDeErrores>();
builder.Services.AddCors(opciones => opciones.AddDefaultPolicy(politica =>
    politica.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

WebApplication app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCors();

await app.Services.CrearBaseDeDatosAsync();

app.MapCatalogo();
app.MapAdministracion();

app.Run();
