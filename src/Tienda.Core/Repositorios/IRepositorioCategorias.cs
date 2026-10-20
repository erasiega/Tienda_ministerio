using Tienda.Core.Entidades;

namespace Tienda.Core.Repositorios;

/// <summary>Acceso a las categorías. El núcleo solo conoce esta interfaz; la implementa <c>Tienda.Datos</c>.</summary>
public interface IRepositorioCategorias
{
    /// <summary>Lista todas las categorías, ordenadas por nombre.</summary>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>Las categorías.</returns>
    Task<IReadOnlyList<Categoria>> ListarAsync(CancellationToken cancelacion = default);

    /// <summary>Busca una categoría por su slug.</summary>
    /// <param name="slug">Slug en minúsculas, por ejemplo «neveras».</param>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>La categoría, o <c>null</c> si no existe.</returns>
    Task<Categoria?> ObtenerPorSlugAsync(string slug, CancellationToken cancelacion = default);
}
