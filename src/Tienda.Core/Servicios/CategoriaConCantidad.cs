using Tienda.Core.Entidades;

namespace Tienda.Core.Servicios;

/// <summary>Una categoría con la cantidad de productos activos que tiene (CA-01).</summary>
/// <param name="Categoria">La categoría.</param>
/// <param name="CantidadProductos">Productos activos de la categoría.</param>
public record CategoriaConCantidad(Categoria Categoria, int CantidadProductos);
