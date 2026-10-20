namespace Tienda.Contratos;

/// <summary>Un producto en una lista del catálogo.</summary>
/// <param name="Id">Identificador.</param>
/// <param name="Sku">Código del producto.</param>
/// <param name="Nombre">Nombre comercial.</param>
/// <param name="Categoria">Slug de la categoría.</param>
/// <param name="Precio">Precio de venta en pesos.</param>
/// <param name="Disponible">Si hay al menos una unidad.</param>
/// <param name="ImagenUrl">Ruta de la imagen.</param>
public record ProductoResumenRespuesta(int Id, string Sku, string Nombre, string Categoria, decimal Precio, bool Disponible, string ImagenUrl);
