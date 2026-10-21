using Tienda.Contratos;
using Tienda.Core.Entidades;
using Tienda.Core.Servicios;

namespace Tienda.Api.Mapeadores;

internal static class MapeadorProducto
{
    internal static ProductoResumenRespuesta AResumen(Producto producto) =>
        new(producto.Id, producto.Sku, producto.Nombre, producto.Categoria?.Slug ?? string.Empty,
            producto.PrecioVenta, producto.Disponible, producto.ImagenUrl);

    internal static ProductoDetalleRespuesta ADetalle(Producto producto) =>
        new(producto.Id, producto.Sku, producto.Nombre, producto.Descripcion,
            producto.Categoria?.Slug ?? string.Empty, producto.PrecioVenta, producto.Disponible, producto.ImagenUrl);

    internal static PaginaRespuesta<ProductoResumenRespuesta> APaginaResumen(Pagina<Producto> pagina)
    {
        List<ProductoResumenRespuesta> elementos = pagina.Elementos.Select(AResumen).ToList();
        int totalPaginas = (int)Math.Ceiling(pagina.Total / (double)pagina.Tamano);
        return new PaginaRespuesta<ProductoResumenRespuesta>(
            elementos, pagina.Total, pagina.NumeroPagina, pagina.Tamano, totalPaginas);
    }
}
