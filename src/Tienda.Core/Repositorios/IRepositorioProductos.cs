using Tienda.Core.Entidades;

namespace Tienda.Core.Repositorios;

/// <summary>Acceso a los productos. El núcleo solo conoce esta interfaz; la implementa <c>Tienda.Datos</c>.</summary>
public interface IRepositorioProductos
{
    /// <summary>Busca un producto por su identificador, esté activo o no.</summary>
    /// <param name="id">Identificador.</param>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>El producto con su categoría, o <c>null</c> si no existe.</returns>
    Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancelacion = default);

    /// <summary>Busca productos activos (R-08), ordenados por nombre, de a una página.</summary>
    /// <param name="categoriaId">Categoría; <c>null</c> para todas.</param>
    /// <param name="texto">Texto que debe estar en el nombre, sin importar mayúsculas; <c>null</c> para no filtrar.</param>
    /// <param name="saltar">Cuántos productos saltar.</param>
    /// <param name="tomar">Cuántos productos traer.</param>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Los productos de la página y el total que cumple el filtro.</returns>
    Task<(IReadOnlyList<Producto> Productos, int Total)> BuscarActivosAsync(int? categoriaId, string? texto,
        int saltar, int tomar, CancellationToken cancelacion = default);

    /// <summary>Cuenta los productos activos de cada categoría (R-08).</summary>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Para cada Id de categoría, cuántos productos activos tiene.</returns>
    Task<IReadOnlyDictionary<int, int>> ContarActivosPorCategoriaAsync(CancellationToken cancelacion = default);
}
