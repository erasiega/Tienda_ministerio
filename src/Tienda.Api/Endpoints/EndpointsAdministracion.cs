using Tienda.Contratos;
using Tienda.Core.Entidades;
using Tienda.Core.Servicios;
using Tienda.Api.Mapeadores;

namespace Tienda.Api.Endpoints;

/// <summary>Endpoints del personal de la tienda. Piden la clave de administración en el encabezado <c>X-Clave-Admin</c> (R-10).</summary>
public static class EndpointsAdministracion
{
    /// <summary>Registra los endpoints de administración bajo <c>/api/admin</c>.</summary>
    /// <param name="app">Rutas de la aplicación.</param>
    /// <returns>Las mismas rutas, para encadenar llamadas.</returns>
    public static IEndpointRouteBuilder MapAdministracion(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/admin/productos/{id:int}/precio", async (int id, CambiarPrecioSolicitud solicitud,
            HttpRequest peticion, IConfiguration configuracion, ServicioCatalogo servicio) =>
        {
            string? clave = peticion.Headers["X-Clave-Admin"];
            if (clave != configuracion["Administracion:Clave"])
            {
                return Results.Problem(statusCode: StatusCodes.Status401Unauthorized, title: "No autorizado",
                    detail: "Falta la clave de administración o no es correcta (R-10).");
            }

            Producto producto = await servicio.CambiarPrecioAsync(id, solicitud.Precio);
            return Results.Ok(MapeadorProducto.ADetalle(producto));
        });

        return app;
    }
}
