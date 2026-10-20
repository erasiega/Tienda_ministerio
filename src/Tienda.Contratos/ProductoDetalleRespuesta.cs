namespace Tienda.Contratos;

/// <summary>El detalle de un producto. No incluye datos internos como el costo de compra (CA-07).</summary>
/// <param name="Id">Identificador.</param>
/// <param name="Sku">Código del producto.</param>
/// <param name="Nombre">Nombre comercial.</param>
/// <param name="Descripcion">Descripción.</param>
/// <param name="Categoria">Slug de la categoría.</param>
/// <param name="Precio">Precio de venta en pesos.</param>
/// <param name="Disponible">Si hay al menos una unidad (P-06).</param>
/// <param name="ImagenUrl">Ruta de la imagen.</param>
public record ProductoDetalleRespuesta(int Id, string Sku, string Nombre, string Descripcion, string Categoria, decimal Precio, bool Disponible, string ImagenUrl);
