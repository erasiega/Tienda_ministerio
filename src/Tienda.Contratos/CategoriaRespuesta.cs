namespace Tienda.Contratos;

/// <summary>Una categoría del catálogo, como la ve el cliente.</summary>
/// <param name="Id">Identificador.</param>
/// <param name="Nombre">Nombre que ve el visitante.</param>
/// <param name="Slug">Identificador para la dirección web.</param>
/// <param name="CantidadProductos">Productos activos de la categoría.</param>
public record CategoriaRespuesta(int Id, string Nombre, string Slug, int CantidadProductos);
