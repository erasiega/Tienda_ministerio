using Tienda.Api.Mapeadores;
using Tienda.Contratos;
using Tienda.Core.Entidades;
using Tienda.Core.Servicios;

namespace Tienda.Api.Endpoints;

/// <summary>Endpoints de pedidos: hacer un pedido y consultarlo con su código.</summary>
public static class EndpointsPedidos
{
    /// <summary>Registra los endpoints de pedidos bajo <c>/api/pedidos</c>.</summary>
    /// <param name="app">Rutas de la aplicación.</param>
    /// <returns>Las mismas rutas, para encadenar llamadas.</returns>
    public static IEndpointRouteBuilder MapPedidos(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder grupo = app.MapGroup("/api/pedidos");

        grupo.MapPost("/", async (CrearPedidoSolicitud solicitud, ServicioPedidos servicio) =>
        {
            Pedido pedido = await servicio.RegistrarAsync(MapeadorPedido.ADatos(solicitud));
            return Results.Created($"/api/pedidos/{pedido.Codigo}", MapeadorPedido.ARespuesta(pedido));
        });

        grupo.MapGet("/{codigo}", async (string codigo, ServicioPedidos servicio) =>
            Results.Ok(MapeadorPedido.ARespuesta(await servicio.ObtenerPorCodigoAsync(codigo))));

        return app;
    }
}
