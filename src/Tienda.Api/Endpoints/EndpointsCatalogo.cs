using Tienda.Contratos;
using Tienda.Core.Entidades;
using Tienda.Core.Servicios;
using Tienda.Api.Mapeadores;

namespace Tienda.Api.Endpoints;

/// <summary>Endpoints públicos del catálogo: categorías y productos.</summary>
public static class EndpointsCatalogo
{
    /// <summary>Registra los endpoints del catálogo bajo <c>/api</c>.</summary>
    /// <param name="app">Rutas de la aplicación.</param>
    /// <returns>Las mismas rutas, para encadenar llamadas.</returns>
    public static IEndpointRouteBuilder MapCatalogo(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder grupo = app.MapGroup("/api");

        grupo.MapGet("/categorias", async (ServicioCatalogo servicio) =>
        {
            IReadOnlyList<CategoriaConCantidad> categorias = await servicio.ListarCategoriasAsync();
            return Results.Ok(categorias.Select(item =>
                new CategoriaRespuesta(item.Categoria.Id, item.Categoria.Nombre, item.Categoria.Slug, item.CantidadProductos)));
        });

        grupo.MapGet("/productos", async (ServicioCatalogo servicio, string? categoria, string? buscar, int? pagina, int? tamano) =>
        {
            Pagina<Producto> resultado = await servicio.BuscarProductosAsync(categoria, buscar,
                pagina ?? ConstantesPaginacion.PaginaPredeterminada,
                tamano ?? ConstantesPaginacion.TamanoPaginaPredeterminado);
            return Results.Ok(MapeadorProducto.APaginaResumen(resultado));
        });

        grupo.MapGet("/categorias/{slug}/productos", async (ServicioCatalogo servicio, string slug, int? pagina, int? tamano) =>
        {
            Pagina<Producto> resultado = await servicio.BuscarProductosAsync(slug, null,
                pagina ?? ConstantesPaginacion.PaginaPredeterminada,
                tamano ?? ConstantesPaginacion.TamanoPaginaPredeterminado);
            return Results.Ok(MapeadorProducto.APaginaResumen(resultado));
        });

        grupo.MapGet("/productos/{id:int}", async (ServicioCatalogo servicio, int id) =>
            Results.Ok(MapeadorProducto.ADetalle(await servicio.ObtenerProductoAsync(id))));

        return app;
    }
}
