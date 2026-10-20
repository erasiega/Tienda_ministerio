using Tienda.Core.Entidades;
using Tienda.Core.Repositorios;

namespace Tienda.Pruebas.Falsos;

/// <summary>Repositorio de categorías en memoria.</summary>
internal sealed class RepositorioCategoriasFalso : IRepositorioCategorias
{
    private readonly List<Categoria> _categorias = [];

    public Categoria Existente(string nombre, string slug)
    {
        var categoria = new Categoria(nombre, slug);
        Ids.Asignar(categoria, _categorias.Count + 1);
        _categorias.Add(categoria);
        return categoria;
    }

    public Task<IReadOnlyList<Categoria>> ListarAsync(CancellationToken cancelacion = default) =>
        Task.FromResult<IReadOnlyList<Categoria>>(_categorias.OrderBy(categoria => categoria.Nombre).ToList());

    public Task<Categoria?> ObtenerPorSlugAsync(string slug, CancellationToken cancelacion = default) =>
        Task.FromResult(_categorias.FirstOrDefault(categoria => categoria.Slug == slug));
}
